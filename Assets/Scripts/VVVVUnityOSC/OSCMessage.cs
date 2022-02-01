#region licence/info
// OSC.NET - Open Sound Control for .NET
// http://luvtechno.net/
//
// Copyright (c) 2006, Yoshinori Kawasaki 
// All rights reserved.
//
// Changes and improvements:
// Copyright (c) 2005-2008 Martin Kaltenbrunner <mkalten@iua.upf.edu>
// As included with    
// http://reactivision.sourceforge.net/
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//
// * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
// * Neither the name of "luvtechno.net" nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS 
// OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
// AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY 
// WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

// adapted by François Zajéga - 2014nov11 - <frankie@frankiezafe.org>
// http://frankiezafe.org

#endregion licence/info

using System;
using System.Collections;
using System.IO;
using System.Text;

namespace VVVV_OSC
{
	/// <summary>
	/// OSCMessage
	/// 
	/// Contains an address, a comma followed by one or more type identifiers. then the data itself follows in binary encoding.
	/// </summary>
	public class OSCMessage : OSCPacket
	{
//      These Attributes adhere to the OSC Specs 1.0
        protected const char INTEGER = 'i'; // int32 8byte
		protected const char FLOAT	  = 'f'; //float32 8byte
		protected const char LONG	  = 'h';  //int64 16byte
		protected const char DOUBLE  = 'd'; // float64 16byte
		protected const char STRING  = 's'; // padded by zeros
		protected const char SYMBOL  = 'S'; // same as STRING really
        protected const char BLOB	  = 'b'; // bytestream, starts with an int that tells the total length of th stream
        protected const char TIMETAG = 't'; // fixed point floating number with 32bytes (16bytes for totaldays after 1.1.1900 and 16bytes for fractionOfDay)
        protected const char CHAR	  = 'c'; // bit
        protected const char COLOR  = 'r'; // 4x8bit -> rgba

        //protected const char TRUE	  = 'T';
        //protected const char FALSE = 'F';
        protected const char NIL = 'N';
        //protected const char INFINITUM = 'I';

        //protected const char ALL     = '*';

//      These Attributes are added for convenience within vvvv. They are NOT part of the OSC Specs, but are VERY useful if you want to make vvvv talk to another instance of vvvv
//      Using them requires to set the ExtendedVVVVMethod property to true (with the constructor or with the Unpack methods, depending if you want to send or receive)
        protected const char VECTOR2D = 'v'; // synonym to dd
        protected const char VECTOR3D = 'V'; // synonym to ddd
        protected const char QUATERNION = 'q'; // synonym to dddd
        protected const char MATRIX4 = 'M';  // for 4x4 Matrices with float, so synonym to ffffffffffffffff


		public OSCMessage(string address ) : base()
		{
            this.typeTag = ",";
			this.Address = address;
		}
		public OSCMessage(string address, object value ) : base()
		{
            this.typeTag = ",";
			this.Address = address;
			Append(value);
		}

		override protected void Pack()
		{
			ArrayList data = new ArrayList();

			AddBytes(data, PackString(this.address));
			PadNull(data);
			AddBytes(data, PackString(this.typeTag));
			PadNull(data);
			
			foreach(object value in this.Values)
			{
				if(value is int) AddBytes(data, PackInt((int)value));
				else if(value is long) AddBytes(data, PackLong((long)value));
				else if(value is float) AddBytes(data, PackFloat((float)value));
				else if(value is double) AddBytes(data, PackDouble((double)value));
				else if(value is string) {
					AddBytes(data, PackString((string)value));
					PadNull(data);
				}
                else if (value is Stream) {
                    AddBytes(data, PackBlob((Stream)value));
                    PadNull(data);
                }
                else if (value is char) AddBytes(data, PackChar((char)value));
                else if (value is DateTime)
                {
                    AddBytes(data, PackTimeTag((DateTime)value));
                }
			}
			
			this.binaryData = (byte[])data.ToArray(typeof(byte));
		}


		public static OSCMessage Unpack(byte[] bytes, ref int start)
		{
			string address = UnpackString(bytes, ref start);
			//Console.WriteLine("address: " + address);
			OSCMessage msg = new OSCMessage( address );

			char[] tags = UnpackString(bytes, ref start).ToCharArray();
			//Console.WriteLine("tags: " + new string(tags));
			foreach(char tag in tags)
			{
				//Console.WriteLine("tag: " + tag + " @ "+start);
				if(tag == ',') continue;
				else if(tag == INTEGER) msg.Append(UnpackInt(bytes, ref start));
				else if(tag == LONG) msg.Append(UnpackLong(bytes, ref start));
				else if(tag == DOUBLE) msg.Append(UnpackDouble(bytes, ref start));
				else if(tag == FLOAT) msg.Append(UnpackFloat(bytes, ref start));
                else if (tag == STRING || tag == SYMBOL) msg.Append(UnpackString(bytes, ref start));
                
                else if (tag == CHAR) msg.Append(UnpackChar(bytes, ref start));
                else if (tag == BLOB) msg.Append(UnpackBlob(bytes, ref start));
                else if (tag == TIMETAG) msg.Append(UnpackTimeTag(bytes, ref start));
			}
			return msg;
		}

		override public void Append(object value)
		{
			if(value is int)
			{
				AppendTag(INTEGER);
			}
			else if(value is long)
			{
				AppendTag(LONG);
			}
			else if(value is float)
			{
				AppendTag(FLOAT);
			}
			else if(value is double)
			{
				AppendTag(DOUBLE);
			}
			else if(value is string)
			{
				AppendTag(STRING);
			}
            else if (value is char)
            {
                AppendTag(CHAR);
            }
            else if (value is Stream)
            {
                AppendTag(BLOB);
            }
            else if (value is DateTime)
            {
                AppendTag(TIMETAG);
            }
            else
            {
                Fallback();
                return;
            }
			values.Add(value);
		}

	    private void Fallback()
	    {
	        AppendTag(NIL);
//	        values.Add("undefined");
	    }

	    protected string typeTag;
		protected void AppendTag(char type)
		{
			typeTag += type;
		}

		override public bool IsBundle() { return false; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Address + " ");
            for(int i = 0; i < values.Count; i++)
                sb.Append(values[i].ToString() + " ");
            return sb.ToString();
        }
	}
}
