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
using System.ComponentModel;
using System.Reflection;
using SharpNekton.Compiler;


namespace SharpNekton.Shared
{
  class Tools
  {

    /// <summary>
    /// Returns a string description of an enumerated value. 
    /// </summary>
    /// <example>
    /// <code>
    /// // An enumeration with added descriptions:
    /// private enum MyColors
    /// {
    ///    [Description("yuk!")]       LightGreen    = 0x012020,
    ///    [Description("nice :-)")]   VeryDeepPink  = 0x123456,
    ///    [Description("so what")]    InvisibleGray = 0x456730,
    ///    [Description("no comment")] DeepestRed    = 0xfafafa,
    ///    [Description("I give up")]  PitchBlack    = 0xffffff,
    /// }
    /// 
    /// // Getting the description:
    /// string descr = Tools.GetDescription( MyColors.LightGreen );  // returns "yuk!"
    /// </code>
    /// </example>
    /// <param name="value">An enumerated value.</param>
    /// <returns>String description of the passed enumerated value.</returns>
    public static string GetDescription(Enum value)
    {
      FieldInfo fi = value.GetType().GetField(value.ToString());
      DescriptionAttribute[] attributes = 
        (DescriptionAttribute[]) fi.GetCustomAttributes( typeof(DescriptionAttribute), false );
      
      return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
    }


    private static int NextChar(string s, int pos)
    {
      if (s == null || (pos < 0 || pos >= s.Length)) return Tokenizer.EOF;   // end of string
      
      return (int) s[pos];
    }


    /** Parses a numeric constant.
     *
     * A numeric constant is defined as:
     *
     * <pre>
     * number :: [ sign ] digit-sequence [ fractional-part ] [ scale-factor ] .
     * fractional-part :: '.' digit-sequence .
     * scale-factor :: ( 'e' | 'E' ) [ scale-factor-sign ]  digit-sequence .
     * digit-sequence :: digit { digit } .
     * scale-factor-sign :: sign
     * sign :: '+' | '-' .
     * digit :: '0' | '1' | '2' | '3' | '4' | '5' | '6' | '7' | '8' | '9' .
     * </pre>
     *
     * @throws Throws an JNektonException with one of the errorcodes included:
     * E_EXPDIGSEQ, E_EXPFRACP, E_EXPSCALEF, if a malformated numeric constant is found.
     *
     * */
    public static double StringToNumber(string s)
    {
      double n, sign;
      int look, pos = 0;

      // remove starting whites
      look = NextChar(s, pos++);
      while (Tokenizer.IsWhite(look) == true) {
        look = NextChar(s, pos++);
      }

      // [ sign ]
      sign = 1.0;
      if (look == '-')
      {
        sign = -1.0;                // set sign
        look = NextChar(s, pos++);  // eat '-'
      }
      else if (look == '+')
      {
        look = NextChar(s, pos++);  // eat '+'
      }

      // digit-sequence expected
      if (Tokenizer.IsDigit(look) == false)
      {
        //throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPDIGSEQ));
        return 0.0;
      }

      // integer-part
      n = 0.0;
      while (Tokenizer.IsDigit(look) == true)
      {
        n *= 10.0;
        n += (double)(look - '0');
        look = NextChar(s, pos++);
      }

      // [ fractional-part ]
      if (look == '.')
      {
        double frac, frac_scale;

        look = NextChar(s, pos++);      // eat '.'

        // digit-sequence expected
        if (Tokenizer.IsDigit(look) == false)
        {
          throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPFRACP));
        }

        // read the fractional-part
        frac = 0.0;
        frac_scale = 0.1;
        while (Tokenizer.IsDigit(look) == true)
        {
          frac += frac_scale * ((double)(look - '0'));
          frac_scale *= 0.1;
          look = NextChar(s, pos++);
        }

        // add fractional-part to the real part
        n += frac;
      }

      // [ scale-factor ]
      if (look == 'e' || look == 'E')
      {
        double sf, sfsign;

        look = NextChar(s, pos++);      // eat the 'e' or the 'E'

        // [ scale-factor-sign ]
        sfsign = 1.0;
        if (look == '-')
        {
          sfsign = -1.0;             // set sign
          look = NextChar(s, pos++);                 // eat '-'
        }
        else if (look == '+')
        {
          look = NextChar(s, pos++);                 // eat '+'
        }

        // digit-sequence expected
        if (Tokenizer.IsDigit(look) == false)
        {
          throw new SharpNektonException(new SharpNektonError(SharpNektonErrorID.E_EXPSCALEF));
        }

        // read the scale-factor
        sf = 0.0;
        while (Tokenizer.IsDigit(look) == true)
        {
          sf *= 10.0;
          sf += (double)(look - '0');
          look = NextChar(s, pos++);
        }

        // aplly a scale-factor-sign to the scale-factor
        sf *= sfsign;

        // apply the scale-factor to the n
        n *= Math.Pow(10.0, sf);
      }

      // aply the sign to the number
      n *= sign;

      // return the parsed number
      return n; 
    }


  } // end of class
} // end of namespace
