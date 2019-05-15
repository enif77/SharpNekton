/* SharpNekton - (C) 2019 Premysl Fara 
 
SharpNekton is available under the zlib license:

This software is provided 'as-is', without any express or implied
warranty.  In no event will the authors be held liable for any damages
arising from the use of this software.

Permission is granted to anyone to use this software for any purpose,
including commercial applications, and to alter it and redistribute it
freely, subject to the following restrictions:

1. The origin of this software must not be misrepresented; you must not
   claim that you wrote the original software. If you use this software
   in a product, an acknowledgment in the product documentation would be
   appreciated but is not required.
2. Altered source versions must be plainly marked as such, and must not be
   misrepresented as being the original software.
3. This notice may not be removed or altered from any source distribution.
 
 */

using System;

namespace SharpNekton.Libraries
{
    class MathLib : ALibrary
    {
        private const double L_E = 2.7182818284590452354;  /* e */
        private const double L_LOG2E = 1.4426950408889634074;    /* log 2e */
        private const double L_LOG10E = 0.43429448190325182765;  /* log 10e */
        private const double L_LN2 = 0.69314718055994530942; /* log e2 */
        private const double L_LN10 = 2.30258509299404568402;    /* log e10 */
        private const double L_PI = 3.14159265358979323846;  /* pi */
        private const double L_PI_2 = 1.57079632679489661923;    /* pi/2 */
        private const double L_PI_4 = 0.78539816339744830962;    /* pi/4 */
        private const double L_1_PI = 0.31830988618379067154;    /* 1/pi */
        private const double L_2_PI = 0.63661977236758134308;    /* 2/pi */
        private const double L_2_SQRTPI = 1.12837916709551257390;    /* 2/sqrt(pi) */
        private const double L_SQRT2 = 1.41421356237309504880;   /* sqrt(2) */
        private const double L_1_SQRT2 = 0.70710678118654752440;     /* 1/sqrt(2) */

        private System.Random rnd;


        public MathLib(ScriptState state) : base(state)
        {
            this.name = "Math";
            this.rnd = new Random();
        }


        public override void Register()
        {
            this.RegisterNumericConstant("L_E", L_E);
            this.RegisterNumericConstant("L_LOG2E", L_LOG2E);
            this.RegisterNumericConstant("L_LOG10E", L_LOG10E);
            this.RegisterNumericConstant("L_LN2", L_LN2);
            this.RegisterNumericConstant("L_LN10", L_LN10);
            this.RegisterNumericConstant("L_PI", L_PI);
            this.RegisterNumericConstant("L_PI_2", L_PI_2);
            this.RegisterNumericConstant("L_PI_4", L_PI_4);
            this.RegisterNumericConstant("L_1_PI", L_1_PI);
            this.RegisterNumericConstant("L_2_PI", L_2_PI);
            this.RegisterNumericConstant("L_2_SQRTPI", L_2_SQRTPI);
            this.RegisterNumericConstant("L_SQRT2", L_SQRT2);
            this.RegisterNumericConstant("L_1_SQRT2", L_1_SQRT2);

            this.RegisterFunction("randomize", f_FnRandomize, -1);
            this.RegisterFunction("rnd", f_FnRnd, -1);

            this.RegisterFunction("trunc", f_FnTrunc, 1);
            this.RegisterFunction("round", f_FnRound, 1);
            this.RegisterFunction("abs", f_FnAbs, 1);
            this.RegisterFunction("sqr", f_FnSqr, 1);
            this.RegisterFunction("sin", f_FnSin, 1);
            this.RegisterFunction("cos", f_FnCos, 1);
            this.RegisterFunction("tan", f_FnTan, 1);
            this.RegisterFunction("asin", f_FnASin, 1);
            this.RegisterFunction("acos", f_FnACos, 1);
            this.RegisterFunction("atan", f_FnATan, 1);
            this.RegisterFunction("atan2", f_FnATan2, 2);
            this.RegisterFunction("sinh", f_FnSinH, 1);
            this.RegisterFunction("cosh", f_FnCosH, 1);
            this.RegisterFunction("tanh", f_FnTanH, 1);
            this.RegisterFunction("exp", f_FnExp, 1);
            this.RegisterFunction("log", f_FnLog, 1);
            this.RegisterFunction("log10", f_FnLog10, 1);
            this.RegisterFunction("pow", f_FnPow, 2);
            this.RegisterFunction("sqrt", f_FnSqrt, 1);
            this.RegisterFunction("ceil", f_FnCeil, 1);
            this.RegisterFunction("floor", f_FnFloor, 1);
            this.RegisterFunction("ldexp", f_FnLdExp, 2);
            this.RegisterFunction("fmod", f_FnfMod, 2);

            this.RegisterFunction("deg", f_FnDeg, 1);
            this.RegisterFunction("rad", f_FnRad, 1);
        }


        public override void UnRegister()
        {
            ; // nothing to be done here
        }

        /*--------------------------------------------------------------------------*/

        /*

        randomize([n])
          Reseeds the random number generator.

        */

        private bool f_FnRandomize(int nparams)
        {
            int seed;

            if (nparams >= 1)
            {
                seed = (int)state.Evaluator.GetNumericParameter(1, nparams);
            }
            else
            {
                seed = (int)System.DateTime.Now.Ticks;
            }

            rnd = new Random(seed);

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = rnd([n])
          Returns a random number.

        */

        private bool f_FnRnd(int nparams)
        {
            double upper, result;

            result = rnd.NextDouble();
            if (nparams >= 1)
            {
                upper = state.Evaluator.GetNumericParameter(1, nparams);
                result = (result % (upper - 1)) + 1;  // FIX: nedava vysledky 1..n, ale 1.x
            }

            state.Evaluator.Return(result);

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = trunc(x)
          Returns the integer portion of the x.
          Ex.: trunc(3.5) = 3; trunc(-3.5) = -3

        */

        private bool f_FnTrunc(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);

            if (dval >= 0.0)
            {
                dval = Math.Floor(dval);
            }
            else
            {
                dval = Math.Ceiling(dval);
            }

            state.Evaluator.Return(dval);

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = round(x)
          Returns the x rounded down to the nearest integer value.
          Ex.: round(3.5) = 4; round(-3.5) = -4

        */

        private bool f_FnRound(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);

            if (dval >= 0.0)
            {
                dval = Math.Floor(dval) + 0.5;
            }
            else
            {
                dval = Math.Ceiling(dval) - 0.5;
            }

            state.Evaluator.Return(dval);

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = abs(x)
          Returns the absolute value (positive value) of the x.
          Ex.: abs(3) = 3; abs(-3) = 3

        */

        private bool f_FnAbs(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return((dval < 0.0) ? -dval : dval);

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = sqr(x)
          Returns the square root of the x.
          Ex.: sqr(3) = 9; sqr(-3) = 9

        */

        private bool f_FnSqr(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return(dval * dval);

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = sin(angle)
          Return the sine of an angle where the angle is expressed in radians.

        */

        private bool f_FnSin(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return(Math.Sin(dval));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = cos(angle)
          Return the cosine of an angle where the angle is expressed in radians.

        */

        private bool f_FnCos(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return(Math.Cos(dval));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = tan(angle)
          Return the tangent of an angle where the angle is expressed in radians.

        */

        private bool f_FnTan(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return(Math.Tan(dval));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = asin(x)
          Return an angle with sine equal to the x. The argument must be
          in the range -1 to +1 inclusive. Returned angle is expressed in radians.

        */

        private bool f_FnASin(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return(Math.Asin(dval));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = acos(x)
          Return an angle with cosine equal to the x. The argument must be
          in the range -1 to +1 inclusive. Returned angle is expressed in radians.

        */

        private bool f_FnACos(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return(Math.Acos(dval));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = atan(x)
          Return an angle with tangent equal to the x. A value between -PI/2 and PI/2
          (in radians) will be returned.

        */

        private bool f_FnATan(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return(Math.Atan(dval));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = atan2(x, y)
          This function computes the angle, in the range [-Pi..Pi] radians, whose
          tangent is y/x.

        */

        private bool f_FnATan2(int nparams)
        {
            double x = state.Evaluator.GetNumericParameter(1, nparams);
            double y = state.Evaluator.GetNumericParameter(2, nparams);
            state.Evaluator.Return(Math.Atan2(x, y));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = sinh(angle)
          Returns the hyperbolic sine of the angle.

        */

        private bool f_FnSinH(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return(Math.Sinh(dval));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = cosh(angle)
            Returns the hyperbolic cosine of the angle.

        */

        private bool f_FnCosH(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return(Math.Cosh(dval));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = tanh(angle)
          Returns the hyperbolic tangent of the angle.

        */

        private bool f_FnTanH(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return(Math.Tanh(dval));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = exp(x)
          This function computes the exponential of x, e^x, where e is the base
          of the natural system of logarithms, approximately 2.718281828.

        */

        private bool f_FnExp(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return(Math.Exp(dval));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = log(x)
          Returns the natural logarithm of x.

        */

        private bool f_FnLog(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return(Math.Log(dval));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = log10(x)
          Returns the base-10 logarithm of x.

        */

        private bool f_FnLog10(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return(Math.Log10(dval));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = pow(x, y)
          Returns x**y, x raised to the power y.

        */

        private bool f_FnPow(int nparams)
        {
            double x = state.Evaluator.GetNumericParameter(1, nparams);
            double y = state.Evaluator.GetNumericParameter(2, nparams);
            state.Evaluator.Return(Math.Pow(x, y));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = sqrt(x)
          Returns the square root of x.

        */

        private bool f_FnSqrt(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return(Math.Sqrt(dval));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = ceil(x)
          Returns the smallest integer greater than or equal to x.

        */

        private bool f_FnCeil(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return(Math.Ceiling(dval));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = floor(x)
          Returns the largest integer not greater than x.

        */

        private bool f_FnFloor(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return(Math.Floor(dval));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = ldexp(x, n)
           Returns x * 2 ** n.

        */

        private bool f_FnLdExp(int nparams)
        {
            double x = state.Evaluator.GetNumericParameter(1, nparams);
            double n = state.Evaluator.GetNumericParameter(2, nparams);
            state.Evaluator.Return(x * Math.Pow(2, n));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = fmod(x, y)
          Returns the remainder of x/y, which is x - iy for some integer i such
          that iy < x < (i+1)y.

        */

        private bool f_FnfMod(int nparams)
        {
            double x = state.Evaluator.GetNumericParameter(1, nparams);
            double y = state.Evaluator.GetNumericParameter(2, nparams);
            state.Evaluator.Return(Math.IEEERemainder(x, y));

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = rad(degrees)
          Returns an angle expressed in degrees converted to radians:
          (degrees / 180.0) * PI

        */

        private bool f_FnRad(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return((dval / 180.0) * L_PI);

            return true;
        }

        /*--------------------------------------------------------------------------*/

        /*

        number = deg(radians)
          Returns an angle expressed in radians converted to degrees:
          (radians / PI) * 180

        */

        private bool f_FnDeg(int nparams)
        {
            double dval = state.Evaluator.GetNumericParameter(1, nparams);
            state.Evaluator.Return((dval / L_PI) * 180.0);

            return true;
        }

    } // end of class
} // end of namespace
