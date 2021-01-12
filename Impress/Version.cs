using System;
using System.Collections.Generic;
using System.Text;

namespace Impress
{
    /// <summary>
    /// Represent a software version with the standard convention fields: Manjor, Minor, Revision , Build and Variant
    /// 
    /// Major and minor are always required.
    /// 
    /// Can to used to parse from a version formated string (see Parse)
    /// 
    /// 
    /// 
    /// </summary>
    public struct Version : IEquatable<Version>, IComparable<Version>
    {
        public static bool operator <(Version left, Version right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(Version left, Version right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(Version left, Version right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(Version left, Version right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static Version Of(int major, int minor, int revision = 0, int build = 0, string variant = null)
        {
            return new Version(major, minor, revision, build, variant);
        }

        /// <summary>
        /// Parse a string in the format 
        /// 
        ///  M.m[.r][.b][-variant]
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Version Parse(string text)
        {

            if (text == null)
            {
                throw new ArgumentNullException("Version text cannot be null");
            }

            if (!text.Contains("."))
            {
                throw new FormatException("Version text is not in the expected format. Major and minor are mandatory");
            }

            string variant = null;
            if (text.Contains("-"))
            {
                var s = text.Split('-');
                variant = s[1];
                text = s[0];
            }

            var parts = new List<string>(text.Split('.'));

            while (parts.Count < 4)
            {
                parts.Add("0");
            }

            return new Version(
                int.Parse(parts[0]),
                int.Parse(parts[1]),
                int.Parse(parts[2]),
                int.Parse(parts[3]),
                variant
            );

        }

        public int Major { get; private set; }
        public int Minor { get; private set; }
        public int Revision { get; private set; }
        public int Build { get; private set; }
        public string Variant { get; private set; }

        private Version(int major, int minor, int revision, int build, string variant)
        {
            Major = major;
            Minor = minor;
            Revision = revision;
            Build = build;
            Variant = variant;
        }

        public override string ToString()
        {

            var version = "{0}.{1}.{2}".Interpolate(Major, Minor, Revision);

            if (Build > 0)
            {
                version += "." + Build.ToString();
            }


            if (Variant != null)
            {
                version += "-" + Variant.ToString();
            }

            return version;
        }


        public override bool Equals(object obj)
        {
            return obj is Version && Equals((Version)obj);
        }

        public bool Equals(Version other)
        {
            return this.Major == other.Major
                && this.Minor == other.Minor
                && this.Revision == other.Revision
                && this.Build == other.Build
                && this.Variant == other.Variant;
        }

        /// <summary>
        /// Tests if this is equal to other ignoring the build and variant fields.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool EqualsIgnoreBuild(Version other)
        {
            return this.Major == other.Major
                && this.Minor == other.Minor
                && this.Revision == other.Revision;
        }

        public override int GetHashCode()
        {
            return Hash.Create(Major).Add(Minor).Add(Revision).GetHashCode();
        }

        public int CompareTo(Version other)
        {
            var comp = this.Major.CompareTo(other.Major);
            if (comp == 0)
            {
                comp = this.Minor.CompareTo(other.Minor);
                if (comp == 0)
                {
                    comp = this.Revision.CompareTo(other.Revision);
                    if (comp == 0)
                    {

                    }
                }
            }
            return comp;
        }
    }
}
