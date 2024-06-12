// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.AnyRadix
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;

namespace BR.AN.PviServices
{
  public class AnyRadix : ICustomFormatter, IFormatProvider
  {
    private const string radixCode = "Ra";
    private static char[] rDigits = new char[36]
    {
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F',
      'G',
      'H',
      'I',
      'J',
      'K',
      'L',
      'M',
      'N',
      'O',
      'P',
      'Q',
      'R',
      'S',
      'T',
      'U',
      'V',
      'W',
      'X',
      'Y',
      'Z'
    };

    public object GetFormat(Type argType) => argType == typeof (ICustomFormatter) ? (object) this : (object) null;

    public string Format(string formatString, object argToBeFormatted, IFormatProvider provider)
    {
      if (formatString == null || !formatString.Trim().StartsWith("Ra"))
        return argToBeFormatted is IFormattable ? ((IFormattable) argToBeFormatted).ToString(formatString, provider) : argToBeFormatted.ToString();
      char[] chArray = new char[63];
      formatString = formatString.Replace("Ra", "");
      long int64;
      try
      {
        int64 = System.Convert.ToInt64(formatString);
      }
      catch (System.Exception ex)
      {
        throw new ArgumentException(string.Format("The radix \"{0}\" is invalid.", (object) formatString), ex);
      }
      if (int64 >= 2L)
      {
        if (int64 <= 36L)
        {
          long num1;
          try
          {
            num1 = (long) argToBeFormatted;
          }
          catch (System.Exception ex)
          {
            throw new ArgumentException(string.Format("The argument \"{0}\" cannot be converted to an integer value.", argToBeFormatted), ex);
          }
          long num2 = Math.Abs(num1);
          int length;
          for (length = 0; length <= 64 && num2 != 0L; ++length)
          {
            chArray[chArray.Length - length - 1] = AnyRadix.rDigits[num2 % int64];
            num2 /= int64;
          }
          if (num1 < 0L)
            chArray[chArray.Length - length++ - 1] = '-';
          return new string(chArray, chArray.Length - length, length);
        }
      }
      throw new ArgumentException(string.Format("The radix \"{0}\" is not in the range 2..36.", (object) formatString));
    }
  }
}
