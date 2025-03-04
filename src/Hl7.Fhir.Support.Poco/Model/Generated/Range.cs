// <auto-generated/>
// Contents of: hl7.fhir.r5.core version: 5.0.0-snapshot1

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Hl7.Fhir.Introspection;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Specification;
using Hl7.Fhir.Utility;
using Hl7.Fhir.Validation;

/*
  Copyright (c) 2011+, HL7, Inc.
  All rights reserved.
  
  Redistribution and use in source and binary forms, with or without modification, 
  are permitted provided that the following conditions are met:
  
   * Redistributions of source code must retain the above copyright notice, this 
     list of conditions and the following disclaimer.
   * Redistributions in binary form must reproduce the above copyright notice, 
     this list of conditions and the following disclaimer in the documentation 
     and/or other materials provided with the distribution.
   * Neither the name of HL7 nor the names of its contributors may be used to 
     endorse or promote products derived from this software without specific 
     prior written permission.
  
  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
  IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
  NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
  POSSIBILITY OF SUCH DAMAGE.
  
*/

namespace Hl7.Fhir.Model
{
  /// <summary>
  /// Set of values bounded by low and high
  /// </summary>
  [Serializable]
  [DataContract]
  [FhirType("Range","http://hl7.org/fhir/StructureDefinition/Range")]
  public partial class Range : Hl7.Fhir.Model.DataType
  {
    /// <summary>
    /// FHIR Type Name
    /// </summary>
    public override string TypeName { get { return "Range"; } }

    /// <summary>
    /// Low limit
    /// </summary>
    [FhirElement("low", InSummary=true, Order=30)]
    [DataMember]
    public Hl7.Fhir.Model.Quantity Low
    {
      get { return _Low; }
      set { _Low = value; OnPropertyChanged("Low"); }
    }

    private Hl7.Fhir.Model.Quantity _Low;

    /// <summary>
    /// High limit
    /// </summary>
    [FhirElement("high", InSummary=true, Order=40)]
    [DataMember]
    public Hl7.Fhir.Model.Quantity High
    {
      get { return _High; }
      set { _High = value; OnPropertyChanged("High"); }
    }

    private Hl7.Fhir.Model.Quantity _High;

    public override IDeepCopyable CopyTo(IDeepCopyable other)
    {
      var dest = other as Range;

      if (dest == null)
      {
        throw new ArgumentException("Can only copy to an object of the same type", "other");
      }

      base.CopyTo(dest);
      if(Low != null) dest.Low = (Hl7.Fhir.Model.Quantity)Low.DeepCopy();
      if(High != null) dest.High = (Hl7.Fhir.Model.Quantity)High.DeepCopy();
      return dest;
    }

    public override IDeepCopyable DeepCopy()
    {
      return CopyTo(new Range());
    }

    ///<inheritdoc />
    public override bool Matches(IDeepComparable other)
    {
      var otherT = other as Range;
      if(otherT == null) return false;

      if(!base.Matches(otherT)) return false;
      if( !DeepComparable.Matches(Low, otherT.Low)) return false;
      if( !DeepComparable.Matches(High, otherT.High)) return false;

      return true;
    }

    public override bool IsExactly(IDeepComparable other)
    {
      var otherT = other as Range;
      if(otherT == null) return false;

      if(!base.IsExactly(otherT)) return false;
      if( !DeepComparable.IsExactly(Low, otherT.Low)) return false;
      if( !DeepComparable.IsExactly(High, otherT.High)) return false;

      return true;
    }

    [IgnoreDataMember]
    public override IEnumerable<Base> Children
    {
      get
      {
        foreach (var item in base.Children) yield return item;
        if (Low != null) yield return Low;
        if (High != null) yield return High;
      }
    }

    [IgnoreDataMember]
    public override IEnumerable<ElementValue> NamedChildren
    {
      get
      {
        foreach (var item in base.NamedChildren) yield return item;
        if (Low != null) yield return new ElementValue("low", Low);
        if (High != null) yield return new ElementValue("high", High);
      }
    }

  }

}

// end of file
