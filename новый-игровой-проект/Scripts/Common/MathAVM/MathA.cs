using System;
using System.Collections.Generic;
using System.Linq;

using Godot;

namespace DustyStudios.MathAVM
{
    public enum RoundMode
    {
        min, normal
    }
    public static class MathA
    {
        public static class Angle
        {

            public static float MoveTowards(float from, float to, float delta) => Clamp(from + MoveTowardsDiff(from, to, delta));
            public static float MoveTowardsDiff(float from, float to, float delta) => Math.Clamp(Diff(to, from), -delta, delta);
            public static float Diff(float a, float b) => Clamp(a - b);
            public static float Clamp(float a) => (a + 180) % 360 - 180;
        }

        private static IEnumerator<Vector2I> OrderByLength(params Vector2I[] args) => args.OrderByDescending(arg => arg.LengthSquared()).GetEnumerator();

        public static Vector2I Longest(params Vector2I[] args)
        {
            Vector2I longest = args[0];
            for(int i = 1; i < args.Length; i++)
                longest = Longer(longest, args[i]);
            return longest;
        }
        public static Vector2I Longer(Vector2I a, Vector2I b) => a.LengthSquared() > b.LengthSquared() ? a : b;

        public static Vector2I Shortest(params Vector2I[] args)
        {
            Vector2I shortest = args[0];
            for(int i = 1; i < args.Length; i++)
                shortest = Shorter(shortest, args[i]);
            return shortest;
        }
        public static Vector2I Shorter(Vector2I a, Vector2I b) => a.LengthSquared() < b.LengthSquared() ? a : b;

        private static IEnumerator<Vector2> OrderByMagnitude(params Vector2[] args) => args.OrderByDescending(arg => arg.LengthSquared()).GetEnumerator();

        public static Vector2 Longest(params Vector2[] args)
        {
            Vector2 longest = args[0];
            for(int i = 1; i < args.Length; i++)
                longest = Longer(longest, args[i]);
            return longest;
        }
        public static Vector2 Longer(Vector2 a, Vector2 b) => a.LengthSquared() > b.LengthSquared() ? a : b;

        public static Vector2 Shortest(params Vector2[] args)
        {
            Vector2 shortest = args[0];
            for(int i = 1; i < args.Length; i++)
                shortest = Shorter(shortest, args[i]);
            return shortest;
        }
        public static Vector2 Shorter(Vector2 a, Vector2 b) => a.LengthSquared() < b.LengthSquared() ? a : b;
        public static Vector2 RotatedVector(Vector2 VectorToRotate, float angle)
        {
            float cos = Mathf.DegToRad(angle), sin = Mathf.Sin(cos);
            cos = Mathf.Cos(cos);
			return new Vector2(
					VectorToRotate.X * cos - VectorToRotate.Y * sin,
					VectorToRotate.X * sin + VectorToRotate.Y * cos
				);
        }

		public static Vector2 RotatedVector(Vector2 VectorToRotate, Vector2 angleExample) => angleExample.Normalized() * VectorToRotate.Length();
		public static Quaternion VectorsAngle(Vector2 vector) => Quaternion.FromEuler(new(0, 0, Mathf.RadToDeg(Mathf.Atan2(vector.Y, vector.X))));


		public static Color ColorBetweenTwoOther(Color first, Color second, int startOfTransformation, int endOfTransformation, int proportion) => first.Lerp(second, (float)((float)(Math.Clamp(proportion, startOfTransformation, endOfTransformation) - startOfTransformation) /
							   (endOfTransformation - startOfTransformation)));
		public static sbyte OneOrNegativeOne(float number) => number >= 0 ? (sbyte)1 : (sbyte)-1;

        public static sbyte OneOrNegativeOne(bool boolean) => !boolean ? (sbyte)1 : (sbyte)-1;
        public const double PHI = 1.61803398874989484820458683436563811772030917980576286213544862270526046281890;

        public static readonly (string symbol, double value)[] Constants = new[] {
                ("π", Math.PI),
                ("φ", PHI),
                ("e", Math.E)
            };
        [DustyConsoleCommand("number", "I just want you to look at my system for converting numbers to text ^υ^", typeof(double))]
        public static string NumberToString(double number)
        {
            const float tolerance = 0.05f;
            if(number == 0) return "0";
            else if(double.IsNaN(number)) return "?";
            else if(number == double.PositiveInfinity) return "∞";
            else if(number == double.NegativeInfinity) return "-∞";
            else if(number == float.Epsilon) return "ε";
            else if(number == -float.Epsilon) return "-ε";
            else if(number == double.MaxValue || number == float.MaxValue || number == int.MaxValue || number == long.MaxValue) return "Gazillion";
            else if(number == double.MinValue || number == float.MinValue || number == int.MinValue || number == long.MinValue) return "-Gazillion";
            else
            {
                string multiplierConsts = "";

                foreach(var constant in Constants)
                    ParseConstant(constant.symbol, constant.value);
                void ParseConstant(string symbol, double value)
                {
                    if(Math.Abs(number) % value <= tolerance)
                    {
                        number /= value;
                        multiplierConsts += symbol;
                    }
                }
                string Number = number.ToString().Take(5).Aggregate("",(x,y) => (string)x+y);
                return Number + multiplierConsts;
            }
        }
        public static readonly string[] MaxValueNames = { "Google", "MaxValue","Gazillion", "Morbillion", "Kajillion", "Bajillion", "Squillion", "Zinglezangillion", "Baggillion", "Zentillion", "Дофигаллион", "Морбиллион", "Дохуяллион", "Хрензнаетскольколлион", "Максимальное число","Многа","Ундециллион","Два ундециллиона" };
        public static bool TryStringToNumber(string str, out double number)
        {
            number = 666;
            str = str.Replace('.', ',');
            if(str == "?") number = double.NaN;
            else if(str == "∞") number = double.PositiveInfinity;
            else if(str == "-∞") number = double.NegativeInfinity;
            else if(str == "ε") number = float.Epsilon;
            else if(str == "-ε") number = -float.Epsilon;
            else if(MaxValueNames.Any(n => n.ToLower() == str.ToLower())) number = double.MaxValue;
            else if(MaxValueNames.Any(n => "-" + n.ToLower() == str.ToLower())) number = double.MinValue;
            else if(str.EndsWith("π")) { string s = str.Replace("π",""); number = double.Parse((s.StartsWith("-") ? "-" : "") + (s.Replace("-", "") != "" ? s.Replace("-", "") : "1")) * Math.PI; } else if(str.EndsWith("φ")) { string s = str.Replace("φ",""); number = double.Parse((s.StartsWith("-") ? "-" : "") + (s.Replace("-", "") != "" ? s.Replace("-", "") : "1")) * PHI; } else if(str.EndsWith("e")) { string s = str.Replace("e",""); number = double.Parse((s.StartsWith("-") ? "-" : "") + (s.Replace("-", "") != "" ? s.Replace("-", "") : "1")) * Math.E; } else if(double.TryParse(str, out double d)) number = d;
            else return false;
            return true;
        }
    }

    [Serializable]
    public class SinusWave
    {
        [Export] public float Amplitude, Length, CurrentPos;

        public float Value => ValueIn(CurrentPos);

        public float ValueIn(float Pos) => Mathf.Sin(Pos * Length) * Amplitude;
    }
}