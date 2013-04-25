/*
Copyright 2013 Surfpup

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
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace Effects
{
    public class Def<T> where T:Codable
    {
        public string name;
        public Type type;
        public Effect<T>.Requirement requirement = (T item) => { return true;};
        public Def(string name, Type type, Effect<T>.Requirement requirement = null)
        {
            this.type=type;
            this.name = type.Namespace+"-"+name;
            if(requirement!=null) this.requirement = requirement;
        }
        public Def(Type type, Effect<T>.Requirement requirement = null)
        {
            this.type=type;
            this.name = type.Namespace+"-"+type.Name;
            if(requirement!=null) this.requirement = requirement;
        }
        public virtual Effect<T> Gen(T item) {
            Effect<T> e = (Effect<T>)Activator.CreateInstance(type, item);
            e.type = this.name;
            return e;
        }
    }
}