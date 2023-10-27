using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab01
{
    public struct Fraction
    {
        private int numerator;
        private int denominator;

        public int Numerator
        {
            get { return numerator; }
            set { numerator = value; }
        }

        public int Denominator
        {
            get { return denominator; }
            set { denominator = value; }
        }

        public Fraction(int n = 0, int d = 0)
        {
            numerator = n;
            if (d == 0)
            {
                d = 1;
            }
            denominator = d;
            Simplify();
        }

        public override string ToString()
        {
            return numerator + "/" + denominator;
        }

        private void Simplify()
        {
            if (denominator < 0)
            {
                denominator *= -1;
                numerator *= -1;
            }
            int gcd = GCD(numerator, denominator);
            numerator /= gcd;
            denominator /= gcd;
        }

        // Regular Call methods
        public static Fraction Multiply(Fraction a, Fraction b)
        {
            int cNum = a.numerator * b.numerator;
            int cDen = a.denominator * b.denominator;
            return new Fraction(cNum, cDen);
        }

        public static Fraction Divide(Fraction a, Fraction b)
        {
            int cNum = a.numerator * b.denominator;
            int cDen = a.denominator * b.numerator;
            return new Fraction(cNum, cDen);
        }

        public static Fraction Addition(Fraction a, Fraction b)
        {
            int cNum = (a.numerator * b.denominator) + (a.denominator * b.numerator);
            int cDen = a.denominator * b.denominator;
            return new Fraction(cNum, cDen);
        }

        public static Fraction Subtraction(Fraction a, Fraction b)
        {
            int cNum = (a.numerator * b.denominator) - (a.denominator * b.numerator);
            int cDen = a.denominator * b.denominator;
            return new Fraction(cNum, cDen);
        }

        // Operator methods
        public static Fraction operator *(Fraction a, Fraction b)
        {
            int cNum = a.numerator * b.numerator;
            int cDen = a.denominator * b.denominator;
            return new Fraction(cNum, cDen);
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            int cNum = a.numerator * b.denominator;
            int cDen = a.denominator * b.numerator;
            return new Fraction(cNum, cDen);
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            int cNum = (a.numerator * b.denominator) + (a.denominator * b.numerator);
            int cDen = a.denominator * b.denominator;
            return new Fraction(cNum, cDen);
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            int cNum = (a.numerator * b.denominator) - (a.denominator * b.numerator);
            int cDen = a.denominator * b.denominator;
            return new Fraction(cNum, cDen);
        }

        public static int GCD(int n, int d)
        {
            if (d == 0)
            {
                return n;
            } else
            {
                return GCD(d, n % d);
            }
        }
    }
}
