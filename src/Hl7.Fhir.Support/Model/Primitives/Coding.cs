/* 
 * Copyright (c) 2015, Firely (info@fire.ly) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.githubusercontent.com/FirelyTeam/fhir-net-api/master/LICENSE
 */

using System;
using System.Collections.Generic;

namespace Hl7.Fhir.Model.Primitives
{

    public struct Coding : IEquatable<Coding>, ICoding
    {
        public Coding(string system, string code, string display = null)
        {
            System = system;
            Code = code; // MV 20200512:  code can be null as well. In old version that was not allowed.
            Display = display;
        }

        public string System { get; }

        public string Code { get; }

        public string Display { get; }

        public override bool Equals(object obj) => obj is Coding coding && Equals(coding);
        public bool Equals(Coding other) => System == other.System && Code == other.Code && Display == other.Display;

        public bool IsEqualTo(Coding other) => throw new NotImplementedException();

        public bool IsEquivalentTo(Coding other) => throw new NotImplementedException();

        public override int GetHashCode()
        {
            var hashCode = -1868345243;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(System);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Code);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Display);
            return hashCode;
        }

        public static bool operator ==(Coding left, Coding right) => left.Equals(right);

        public static bool operator !=(Coding left, Coding right) => !(left == right);

        public static Coding Parse(string value) => throw new NotImplementedException();
        public static bool TryParse(string value, out Coding p) => throw new NotImplementedException();
    }
}
