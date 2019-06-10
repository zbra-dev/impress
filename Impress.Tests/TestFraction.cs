using Impress.Math;
using NUnit.Framework;
using System;

namespace Impress.Tests
{

    public class TestFraction
    {
        [Test]
        public void TestPiRepresentation()
        {
            // from http://www.geom.uiuc.edu/~huberty/math5337/groupe/digits.html
            var pi = "3.141592653589793238462643383279502884197169399375105820974944592307816406286208998628034825342117067982148086513282306647093844609550582231725359408128481117450284102701938521105559644622948954930381964428810975665933446128475648233";

            // absurd accuracy
            var PI = Fraction.ValueOf(pi.Replace(".", ""), "1") / Fraction.ValueOf(10).Pow(pi.Length - 2);

            var diff = PI - Fraction.PI;

            Assert.IsTrue(diff.Abs().ToDecimal() <= 1E-308M);

            var diffToDouble = PI - Fraction.ValueOf(System.Math.PI);

            Assert.IsTrue(diffToDouble.Abs().ToDecimal() <= 1E-15M);
        }

        [Test]
        public void TestERepresentation()
        {
            // from http://www.math.utah.edu/~pa/math/e.html
            var e = "2.718281828459045235360287471352662497757247093699959574966967627724076630353547594571382178525166427427466391932003059921817413596629043572900334295260595630738132328627943490763233829880";

            // absurd accuracy
            var E = Fraction.ValueOf(e.Replace(".", ""), "1") / Fraction.ValueOf(10).Pow(e.Length - 2);


            var diff = E - Fraction.E;

            Assert.IsTrue(diff.Abs() <= Fraction.ValueOf(10).Pow(-38));


            var diffToDouble = E - Fraction.ValueOf(System.Math.E);

            Assert.IsTrue(diffToDouble.Abs().ToDecimal() <= 1E-15M);

        }

        [Test]
        public void TestRepresentation()
        {
            Assert.AreEqual(Fraction.One, Fraction.ValueOf(1));
            Assert.AreEqual(Fraction.One, Fraction.ValueOf(1, 1));
            Assert.AreEqual(Fraction.One, Fraction.ValueOf(2, 2));
            Assert.AreEqual(Fraction.One, Fraction.ValueOf("1"));
            Assert.AreEqual(Fraction.Zero, Fraction.ValueOf("0"));
            Assert.AreEqual(Fraction.Zero, Fraction.ValueOf("0.0"));
            Assert.AreEqual(Fraction.Zero, Fraction.ValueOf(0.0));
            Assert.AreEqual(Fraction.Zero, Fraction.ValueOf(0.0M));
            Assert.AreEqual(Fraction.ValueOf(1, 2), Fraction.ValueOf(5, 10));
            Assert.AreEqual(Fraction.ValueOf(1, 2), Fraction.ValueOf(5, 10));
            Assert.AreEqual(Fraction.ValueOf(1, 3), Fraction.ValueOf(4, 12));
            Assert.AreEqual(Fraction.ValueOf(25, 7), Fraction.ValueOf(75, 21));
            Assert.AreEqual(Fraction.ValueOf(3, 10), Fraction.ValueOf(0.3));
            Assert.AreEqual(Fraction.ValueOf(3, 10), Fraction.ValueOf("0.3"));
            Assert.AreEqual(Fraction.ValueOf(3, 3), Fraction.ValueOf(10, 10));
            Assert.AreEqual(Fraction.ValueOf(-2, 3), Fraction.ValueOf(2, -3));
            Assert.AreEqual(Fraction.ValueOf(2, 3), Fraction.ValueOf(-2, -3));
        }

        [Test]
        public void TestRepresentationMax()
        {
            Assert.AreEqual("179769313486231570814527423731704356798070567525844996598917476803157260780028538760589558632766878171540458953514382464234321326889464182768467546703537516986049910576551282076245490090389328944075868508455133942304583236903222948165808559332123348274797826204144723168738177180919299881250404026184124858368/1", Fraction.ValueOf(Double.MaxValue).ToString());
            Assert.AreEqual("-179769313486231570814527423731704356798070567525844996598917476803157260780028538760589558632766878171540458953514382464234321326889464182768467546703537516986049910576551282076245490090389328944075868508455133942304583236903222948165808559332123348274797826204144723168738177180919299881250404026184124858368/1", Fraction.ValueOf(Double.MinValue).ToString());
            Assert.AreEqual("494065645841247/1000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000", Fraction.ValueOf(Double.Epsilon).ToString());
        }

        [Test]
        public void TestToDecimal()
        {
            var f = Fraction.ValueOf("28955469600404648123603603830873", "67200000000000000000000000000000");
            Assert.AreEqual(0.430884964291737M, f.ToDecimal());

            f = f * -1;
            Assert.AreEqual(-0.430884964291737M, f.ToDecimal());
        }

        [Test]
        public void TestAdd()
        {
            Assert.AreEqual(Fraction.ValueOf(1, 2), Fraction.ValueOf(1, 5) + Fraction.ValueOf(3, 10));
            Assert.AreEqual(Fraction.ValueOf(124, 21), Fraction.ValueOf(25, 7) + Fraction.ValueOf(7, 3));
            Assert.AreEqual(Fraction.ValueOf(7, 5), 1 + Fraction.ValueOf(2, 5));
            Assert.AreEqual(Fraction.ValueOf(7, 5), Fraction.ValueOf(2, 5) + 1);
            Assert.AreEqual(Fraction.ValueOf(12, 5), Fraction.ValueOf(2, 5) + 2);
        }

        [Test]
        public void TestSubtract()
        {
            Assert.AreEqual(Fraction.ValueOf(1, 2), Fraction.ValueOf(1, 5) + Fraction.ValueOf(3, 10));
            Assert.AreEqual(Fraction.ValueOf(124, 21), Fraction.ValueOf(25, 7) + Fraction.ValueOf(7, 3));
            Assert.AreEqual(Fraction.ValueOf(3, 5), 1 - Fraction.ValueOf(2, 5));
            Assert.AreEqual(Fraction.ValueOf(-3, 5), Fraction.ValueOf(2, 5) - 1);
            Assert.AreEqual(Fraction.ValueOf(-8, 5), Fraction.ValueOf(2, 5) - 2);
        }

        [Test]
        public void TestMultiply()
        {
            Assert.AreEqual(Fraction.ValueOf(3, 50), Fraction.ValueOf(1, 5) * Fraction.ValueOf(3, 10));
            Assert.AreEqual(Fraction.ValueOf(1, 3), Fraction.ValueOf(1, 7) * Fraction.ValueOf(7, 3));
            Assert.AreEqual(Fraction.ValueOf(4, 5), Fraction.ValueOf(2, 5) * 2);
            Assert.AreEqual(Fraction.ValueOf(4, 5), Fraction.ValueOf(-2, 5) * -2);
        }

        [Test]
        public void TestDivide()
        {
            Assert.AreEqual(Fraction.ValueOf(5, 3), Fraction.ValueOf(1, 3) / Fraction.ValueOf(1, 5));
            Assert.AreEqual(Fraction.ValueOf(25, 15), Fraction.ValueOf(1, 3) / Fraction.ValueOf(1, 5));
            Assert.AreEqual(Fraction.ValueOf(1, 15), Fraction.ValueOf(1, 3) / Fraction.ValueOf(5));
            Assert.AreEqual(Fraction.ValueOf(1, 15), Fraction.ValueOf(1, 3) / 5);
            Assert.AreEqual(Fraction.ValueOf(15), 5 / Fraction.ValueOf(1, 3));
        }

        [Test]
        public void TestInvert()
        {
            Assert.AreEqual(Fraction.ValueOf(3, 5), Fraction.ValueOf(5, 3).Invert());
            Assert.AreEqual(Fraction.ValueOf(-3, 5), Fraction.ValueOf(-5, 3).Invert());
            Assert.AreEqual(Fraction.ValueOf(-5, 3), Fraction.ValueOf(-5, 3).Invert().Invert());

            Assert.AreEqual(1, Fraction.ValueOf(5, 3).Invert().Sign);
            Assert.AreEqual(-1, Fraction.ValueOf(-5, 3).Invert().Sign);
        }

        [Test]
        public void TestCompare()
        {
            Assert.IsTrue(Fraction.One > Fraction.Zero);
            Assert.IsTrue(Fraction.Zero < Fraction.One);
            Assert.IsTrue(Fraction.One > Fraction.ValueOf(1, 2));
            Assert.IsTrue(Fraction.ValueOf(1, 2) >= Fraction.ValueOf(1, 2));
            Assert.IsTrue(Fraction.ValueOf(1, 2) == Fraction.ValueOf(1, 2));
            Assert.IsTrue(Fraction.ValueOf(1, 2) >= Fraction.ValueOf(2, 5));
            Assert.IsFalse(Fraction.ValueOf(1, 2) >= Fraction.ValueOf(3, 5));

            var a = Fraction.ValueOf(25, 7);
            var b = Fraction.ValueOf(3, 5);

            Assert.IsFalse(a <= b);
        }

        [Test]
        public void TestFromDouble()
        {
            Assert.AreEqual(Fraction.ValueOf(4, 1), Fraction.ValueOf(4.0));
            Assert.AreEqual(Fraction.ValueOf(4, 1000), Fraction.ValueOf(0.004));

            Assert.AreEqual(Fraction.ValueOf(31415926535897931, 10000000000000000), Fraction.ValueOf(System.Math.PI));
            Assert.AreEqual(Fraction.ValueOf(27182818284590451, 10000000000000000), Fraction.ValueOf(System.Math.E));

            var value = 45.89;
            Assert.AreEqual(Fraction.ValueOf((long)(value * 100), 100L), Fraction.ValueOf(value));
            Assert.AreEqual(Fraction.ValueOf((long)(value * 100), 10000L), Fraction.ValueOf(value) / 100);
        }

        [Test]
        public void TestToDouble()
        {
            Assert.AreEqual(2.135, 2135.Over(1000).ToDouble());
            Assert.AreEqual(2.14, 2135.Over(1000).ToDouble(2));
            Assert.AreEqual(2.13, 2131.Over(1000).ToDouble(2));
            Assert.AreEqual(2.14, 2138.Over(1000).ToDouble(2));
        }

        [Test]
        public void TestFloorCeiling()
        {
            Assert.AreEqual(Fraction.Zero, 8.Over(10).Floor);
            Assert.AreEqual(Fraction.One, 8.Over(10).Ceilling);
            Assert.AreEqual(Fraction.One.Negate(), (-8).Over(10).Floor);
            Assert.AreEqual(Fraction.Zero, (-8).Over(10).Ceilling);

            Assert.AreEqual(Fraction.Zero, 2.Over(10).Floor);
            Assert.AreEqual(Fraction.One, 2.Over(10).Ceilling);
            Assert.AreEqual(Fraction.One.Negate(), (-2).Over(10).Floor);
            Assert.AreEqual(Fraction.Zero, (-2).Over(10).Ceilling);

            Assert.AreEqual(Fraction.One, 3.Over(2).Floor);
            Assert.AreEqual(Fraction.ValueOf(2), 3.Over(2).Ceilling);
            Assert.AreEqual(Fraction.ValueOf(-2), (-3).Over(2).Floor);
            Assert.AreEqual(Fraction.One.Negate(), (-3).Over(2).Ceilling);

            Assert.AreEqual(0, 2.Over(10).IntFloor);
            Assert.AreEqual(0, (-2).Over(10).IntFloor);
            Assert.AreEqual(1, 3.Over(2).IntFloor);


            Assert.AreEqual(-1, (-3).Over(2).IntFloor);
            Assert.AreEqual(1, 3.Over(2).IntFloor);

            Assert.AreEqual(1, 2.Over(10).IntCeiling);
            Assert.AreEqual(0, (-2).Over(10).IntCeiling);
            Assert.AreEqual(-1, (-3).Over(2).IntCeiling);
            Assert.AreEqual(2, 3.Over(2).IntCeiling);

        }


        [Test]
        public void TestFromPropertyBag()
        {
            // When reading not initialized properties, the vm silently creates a "default" fraction calling  new Fraction(), but C# does not offer control 
            // over init so the result is a 0/0 fraction
            var bag = new TestBag();

            Assert.IsTrue(bag.Value.IsUndefined());

            // To compensate incorrect struct inicialization by .NET, include the Define method call to ensure the fraction is consistent
            Assert.AreEqual(Fraction.One, bag.Value + Fraction.One);
        }

        [Test]
        public void TestDecrementAndIncrement()
        {
            var t = Fraction.ValueOf(3);
            Assert.AreEqual(Fraction.ValueOf(4), t + 1);
            Assert.AreEqual(Fraction.ValueOf(3), t++); // this increments t to 4 after the assert
            Assert.AreEqual(Fraction.ValueOf(5), ++t);
            Assert.AreEqual(Fraction.ValueOf(4), t - 1);
            Assert.AreEqual(Fraction.ValueOf(5), t--); // this decrements t to 4 after the assert
            Assert.AreEqual(Fraction.ValueOf(3), --t);
            Assert.AreEqual(Fraction.ValueOf(4), 1 + Fraction.ValueOf(3));
            Assert.AreEqual(Fraction.ValueOf(-2), 1 - Fraction.ValueOf(3));
        }
    }


    public class TestBag
    {
        public Fraction Value { get; set; }

    }
}