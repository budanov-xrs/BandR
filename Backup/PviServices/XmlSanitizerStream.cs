// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.XmlSanitizerStream
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.IO;
using System.Text;

namespace BR.AN.PviServices
{
  public class XmlSanitizerStream : StreamReader
  {
    private const int EOF = -1;

    public XmlSanitizerStream(string path)
      : base(path)
    {
    }

    public override int Read()
    {
      int character;
      do
        ;
      while ((character = base.Read()) != -1 && !XmlSanitizerStream.IsLegalXmlChar(character));
      return character;
    }

    public override int Peek()
    {
      int character;
      do
      {
        character = base.Peek();
      }
      while (!XmlSanitizerStream.IsLegalXmlChar(character) && (character = base.Read()) != -1);
      return character;
    }

    public static bool IsLegalXmlChar(int character)
    {
      if (character == 9 || character == 10 || character == 13 || character >= 32 && character <= 55295 || character >= 57344 && character <= 65533)
        return true;
      return character >= 65536 && character <= 1114111;
    }

    public override int Read(char[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (buffer.Length - index < count)
        throw new ArgumentException();
      int num1 = 0;
      do
      {
        int num2 = this.Read();
        if (num2 == -1)
          return num1;
        buffer[index + num1++] = (char) num2;
      }
      while (num1 < count);
      return num1;
    }

    public override int ReadBlock(char[] buffer, int index, int count)
    {
      int num1 = 0;
      int num2;
      do
      {
        num1 += num2 = this.Read(buffer, index + num1, count - num1);
      }
      while (num2 > 0 && num1 < count);
      return num1;
    }

    public override string ReadLine()
    {
      StringBuilder stringBuilder = new StringBuilder();
      int num;
      while (true)
      {
        num = this.Read();
        switch (num)
        {
          case -1:
            goto label_2;
          case 10:
          case 13:
            goto label_5;
          default:
            stringBuilder.Append((char) num);
            continue;
        }
      }
label_2:
      return stringBuilder.Length > 0 ? stringBuilder.ToString() : (string) null;
label_5:
      if (num == 13 && this.Peek() == 10)
        this.Read();
      return stringBuilder.ToString();
    }

    public override string ReadToEnd()
    {
      char[] buffer = new char[4096];
      StringBuilder stringBuilder = new StringBuilder(4096);
      int charCount;
      while ((charCount = this.Read(buffer, 0, buffer.Length)) != 0)
        stringBuilder.Append(buffer, 0, charCount);
      return stringBuilder.ToString();
    }
  }
}
