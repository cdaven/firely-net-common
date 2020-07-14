﻿/* 
 * Copyright (c) 2020, Firely (info@fire.ly) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.githubusercontent.com/FirelyTeam/fhir-net-api/master/LICENSE
 */

#nullable enable

using Hl7.Fhir.Support.Utility;
using System;
using System.Text.RegularExpressions;
using static Hl7.Fhir.Support.Utility.Result;

namespace Hl7.Fhir.Model.Primitives
{
    public class PartialTime : Any, IComparable, ICqlEquatable, ICqlOrderable
    {
        private PartialTime(string original, DateTimeOffset parsedValue, PartialPrecision precision, bool hasOffset)
        {
            _original = original;
            _parsedValue = parsedValue;
            Precision = precision;
            HasOffset = hasOffset;
        }

        public static PartialTime Parse(string representation) =>
            TryParse(representation, out var result) ? result : throw new FormatException($"String '{representation}' was not recognized as a valid partial time.");

        public static bool TryParse(string representation, out PartialTime value) => tryParse(representation, out value);

        public static PartialTime FromDateTimeOffset(DateTimeOffset dto, PartialPrecision prec = PartialPrecision.Fraction,
        bool includeOffset = false)
        {
            string formatString = prec switch
            {
                PartialPrecision.Hour => "HH",
                PartialPrecision.Minute => "HH:mm",
                PartialPrecision.Second => "HH:mm:ss",
                _ => "HH:mm:ss.FFFFFFF",
            };

            if (includeOffset) formatString += "K";

            var representation = dto.ToString(formatString);
            return Parse(representation);
        }

        public static PartialTime Now(bool includeOffset = false) => FromDateTimeOffset(DateTimeOffset.Now, includeOffset: includeOffset);

        public int? Hours => Precision >= PartialPrecision.Hour ? _parsedValue.Hour : (int?)null;
        public int? Minutes => Precision >= PartialPrecision.Minute ? _parsedValue.Minute : (int?)null;
        public int? Seconds => Precision >= PartialPrecision.Second ? _parsedValue.Second : (int?)null;
        public int? Millis => Precision >= PartialPrecision.Fraction ? _parsedValue.Millisecond : (int?)null;

        /// <summary>
        /// The span of time ahead/behind UTC
        /// </summary>
        public TimeSpan? Offset => HasOffset ? _parsedValue.Offset : (TimeSpan?)null;

        private readonly string _original;
        private readonly DateTimeOffset _parsedValue;

        /// <summary>
        /// The precision of the time available. 
        /// </summary>
        public PartialPrecision Precision { get; private set; }

        /// <summary>
        /// Whether the time specifies an offset to UTC
        /// </summary>
        public bool HasOffset { get; private set; }

        // Our regex is pretty flexible, it does not bother to capture rules about semantics (12:64 would be legal here).
        // Additional semantic checks will be verified using the built-in DateTimeOffset .NET parser.
        // Also, it accept the superset of formats specified by FHIR, CQL, FhirPath and the mapping language. Each of these
        // specific implementations may add additional constraints (e.g. about minimum precision or presence of timezones).

        internal static readonly string PARTIALTIMEFORMAT = $"{TIMEFORMAT}{OFFSETFORMAT}?";
        internal const string TIMEFORMAT =
            "(?<time>(?<hours>[0-9][0-9]) ((?<minutes>:[0-9][0-9]) ((?<seconds>:[0-9][0-9]) ((?<frac>.[0-9]+))?)?)?)";
        internal const string OFFSETFORMAT = "(?<offset>Z | (\\+|-) [0-9][0-9]:[0-9][0-9])";

        private static readonly Regex PARTIALTIMEREGEX =
            new Regex("^" + PARTIALTIMEFORMAT + "$",
                RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled | RegexOptions.ExplicitCapture);


        /// <summary>
        /// Converts the partial time to a full DateTimeOffset instance.
        /// </summary>
        /// <param name="year">Year used to turn a time into a date</param>
        /// <param name="month">Month used to turn a time into a date</param>
        /// <param name="day">Day used to turn a time into a date</param>
        /// <param name="defaultOffset">Offset used when the partial time does not specify one.</param>
        /// <returns></returns>
        public DateTimeOffset ToDateTimeOffset(int year, int month, int day, TimeSpan defaultOffset) =>
            new DateTimeOffset(year, month, day, _parsedValue.Hour,
                    _parsedValue.Minute, _parsedValue.Second, _parsedValue.Millisecond,
                    HasOffset ? _parsedValue.Offset : defaultOffset);


        private static bool tryParse(string representation, out PartialTime value)
        {
            if (representation is null) throw new ArgumentNullException(nameof(representation));

            var matches = PARTIALTIMEREGEX.Match(representation);
            if (!matches.Success)
            {
                value = new PartialTime(representation, default, default, default);
                return false;
            }

            var hrg = matches.Groups["hours"];
            var ming = matches.Groups["minutes"];
            var secg = matches.Groups["seconds"];
            var fracg = matches.Groups["frac"];
            var offset = matches.Groups["offset"];

            var prec =
                        fracg.Success ? PartialPrecision.Fraction :
                        secg.Success ? PartialPrecision.Second :
                        ming.Success ? PartialPrecision.Minute :
                        PartialPrecision.Hour;

            var parseableDT = $"2016-01-01T" +
                    (hrg.Success ? hrg.Value : "00") +
                    (ming.Success ? ming.Value : ":00") +
                    (secg.Success ? secg.Value : ":00") +
                    (fracg.Success ? fracg.Value : "") +
                    (offset.Success ? offset.Value : "Z");

            var success = DateTimeOffset.TryParse(parseableDT, out var parsedValue);
            value = new PartialTime(representation, parsedValue, prec, offset.Success);
            return success;
        }

        /// <summary>
        /// Compare two partial times based on CQL equality rules
        /// </summary>
        /// <returns>returns true if the values have the same precision, and each date component is exactly the same. Datetimes with timezones are normalized
        /// to zulu before comparison is done. Throws an <see cref="ArgumentException"/> if the arguments differ in precision.</returns>
        /// <remarks>See <see cref="TryCompareTo(Any)"/> for more details.</remarks>
        public override bool Equals(object obj) => obj is Any other && TryEquals(other).ValueOrDefault(false);

        public Result<bool> TryEquals(Any other) => other is PartialTime ? TryCompareTo(other).Select(i => i == 0) : false;

        public static bool operator ==(PartialTime a, PartialTime b) => Equals(a, b);
        public static bool operator !=(PartialTime a, PartialTime b) => !Equals(a, b);

        /// <summary>
        /// Compare two partial times based on CQL equality rules
        /// </summary>
        /// <remarks>See <see cref="TryCompareTo(Any)"/> for more details.</remarks>
        public int CompareTo(object obj) => obj is PartialTime p ?
            TryCompareTo(p).ValueOrThrow() : throw NotSameTypeComparison(this, obj);
      
        /// <summary>
        /// Compares two (partial)times according to CQL ordering rules.
        /// </summary> 
        /// <param name="other"></param>
        /// <returns>An <see cref="Support.Utility.Ok{T}"/> with an integer value representing the reseult of the comparison: 0 if this and other are equal, 
        /// -1 if this is smaller than other and +1 if this is bigger than other, or the other is null. If the values are incomparable
        /// this function returns a <see cref="Support.Utility.Fail{T}"/> with the reason why the comparison between the two values was impossible.
        /// </returns>
        /// <remarks>The comparison is performed by considering each precision in order, beginning with hours. 
        /// If the values are the same, comparison proceeds to the next precision; 
        /// if the values are different, the comparison stops and the result is false. If one input has a value 
        /// for the precision and the other does not, the comparison stops and the values cannot be compared; if neither
        /// input has a value for the precision, or the last precision has been reached, the comparison stops
        /// and the result is true.</remarks>
        public Result<int> TryCompareTo(Any other)
        {
            return other switch
            {
                null => 1,
                PartialTime p => PartialDateTime.CompareDateTimeParts(_parsedValue, Precision, HasOffset, p._parsedValue, p.Precision, p.HasOffset),
                _ => throw NotSameTypeComparison(this, other)
            };
        }

        public static bool operator <(PartialTime a, PartialTime b) => a.CompareTo(b) < 0;
        public static bool operator <=(PartialTime a, PartialTime b) => a.CompareTo(b) <= 0;
        public static bool operator >(PartialTime a, PartialTime b) => a.CompareTo(b) > 0;
        public static bool operator >=(PartialTime a, PartialTime b) => a.CompareTo(b) >= 0;


        public override int GetHashCode() => _original.GetHashCode();
        public override string ToString() => _original;

        public static explicit operator PartialTime(DateTimeOffset dto) => FromDateTimeOffset(dto);

        bool? ICqlEquatable.IsEqualTo(Any other) => other is { } && TryEquals(other) is Ok<bool> ok ? ok.Value : (bool?)null;

        // Note that, in contrast to equals, this will return false if operators cannot be compared (as described by the spec)
        bool ICqlEquatable.IsEquivalentTo(Any other) => other is { } pd && TryEquals(pd).ValueOrDefault(false);

        int? ICqlOrderable.CompareTo(Any other) => other is { } && TryCompareTo(other) is Ok<int> ok ? ok.Value : (int?)null;
    }
}



