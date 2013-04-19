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
using Microsoft.Xna.Framework;
using Terraria;

namespace Epic_Loot
{
    public class Stat
    {
    	public float min;
    	public float max;
    	public Stat(float min, float max)
    	{
    		this.min=min;
    		this.max=max;
    	}
    	public int IntMin()
    	{
    		return (int) min;
    	}
    	public int IntMax()
    	{
    		return (int) max;
    	}
    	public float Min()
    	{
    		return min;
    	}
    	public float Max()
    	{
    		return max;
    	}
    }

}