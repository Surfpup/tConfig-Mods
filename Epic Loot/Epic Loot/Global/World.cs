/*
Copyright 2012 Surfpup

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections;
using System.IO;
using System.Diagnostics;

using Terraria;

namespace Epic_Loot
{
    public class ModWorld
    {
        public static int modIndex;

        public const int SYNC_AFFIXES_REQUEST = 4;
        public const int SYNC_AFFIXES = 5;

        //Need to add netmessage for when an NPC spawns and affix data needs to be synced
        public static void Initialize(int index)
        {
            modIndex = index;
        }

        public static void NetSendIntercept(int num, ref int num2, ref int num3, int msgType, int remoteClient, int ignoreClient, string text, int number, float number2, float number3, float number4, int number5)
        {
            if (msgType == 23) //NPC update being sent
            { //We'll send affix data over as well
                int npcIndex = number;
                MemoryStream stream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(stream);
                Main.npc[npcIndex].RunMethod("Save", writer);
                num2 += stream.ToArray().Length;
                //Console.WriteLine("Saved affixes for " + npcIndex + ", size=" + stream.ToArray().Length+", index="+(num3+4));

                //Sending the size of the extra data
                byte[] bytes = BitConverter.GetBytes(stream.ToArray().Length);
                Buffer.BlockCopy(bytes, 0, NetMessage.buffer[num].writeBuffer, num3, 4);
                num3+=4;
                num2+=4;
                
                if (stream.ToArray().Length > 0)
                {
                    Buffer.BlockCopy(stream.ToArray(), 0, NetMessage.buffer[num].writeBuffer, num3, stream.ToArray().Length);
                    num3 += stream.ToArray().Length;
                }
            }
        }

        public static void NetReceiveIntercept(messageBuffer msgBuffer, int msgType, int start, int length, ref int num)
        {
            if (msgType == 23 && Main.netMode==1)
            {
                int npcIndex = (int) BitConverter.ToInt16(msgBuffer.readBuffer, start + 1);
                if (num < start + length)
                {
                    int size = BitConverter.ToInt32(msgBuffer.readBuffer, num);
                    num += 4;
                    //Console.WriteLine("NPC Index " + npcIndex + ", size=" + size + ", index=" + num + ", start=" + start + ", length=" + length);

                    if (size > 0)
                    {
                        MemoryStream readStream = new MemoryStream(msgBuffer.readBuffer, num, size);
                    
                        BinaryReader reader = new BinaryReader(readStream);

                        Main.npc[npcIndex].RunMethod("Load", reader, -1);
                        num += size;
                    }
                }
                //else Console.WriteLine("Invalid index: index=" + num + ", start=" + start + ", length=" + length);
            }
        }
    }
}