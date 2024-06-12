// Decompiled with JetBrains decompiler
// Type: BR.AN.PviServices.PlcMessageWindow
// Assembly: BR.AN.PVIServices, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DA853F0A-C070-4E30-B7A7-51D955FB85F6
// Assembly location: C:\Users\ss_six\Desktop\FXE\FXE software integration package\COMET_FXE-PVIServices_Example\Pluto-PVI-trial_01\packages\IToolS.BandR.3.5.58\lib\net20\BR.AN.PVIServices.dll

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BR.AN.PviServices
{
  internal class PlcMessageWindow : Form
  {
    private IntPtr m_pData;
    private uint m_LenOfpData;
    private Service propService;

    public PlcMessageWindow(Service service)
    {
      this.propService = service;
      this.m_pData = IntPtr.Zero;
      this.m_LenOfpData = 0U;
      this.Text = "-§-BR.AN.PviServices MSG Window-$-";
    }

    protected override void WndProc(ref Message msg)
    {
      base.WndProc(ref msg);
      if (msg.Msg < 10000)
        return;
      int error1 = 0;
      IntPtr zero = IntPtr.Zero;
      uint linkID = 0;
      uint pDataLen1 = 0;
      int iWParam = 0;
      int iLParam = 0;
      uint num1 = (uint) (msg.Msg - 10000);
      PviMarshal.WmMsgToInt32(ref msg, ref iWParam, ref iLParam);
      int responseInfo1;
      switch (num1)
      {
        case 97:
          this.propService.DisconnectEx(PviMarshal.WmMsgToUInt32(msg.WParam));
          break;
        case 101:
          ResponseInfo responseInfo2 = new ResponseInfo(0, 0, 0, 0, 0);
          responseInfo1 = PInvokePvicom.PviComGetResponseInfo(this.propService.hPvi, msg.WParam, zero, out pDataLen1, ref responseInfo2, (uint) Marshal.SizeOf(typeof (ResponseInfo)));
          if (pDataLen1 > this.m_LenOfpData)
          {
            if (IntPtr.Zero != this.m_pData)
              PviMarshal.FreeHGlobal(ref this.m_pData);
            this.m_pData = IntPtr.Zero;
            this.m_LenOfpData = pDataLen1;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen1);
          }
          else if (IntPtr.Zero == this.m_pData)
          {
            this.m_LenOfpData = pDataLen1;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen1);
          }
          int num2 = PInvokePvicom.PviComReadResponse(this.propService.hPvi, msg.WParam, this.m_pData, (int) pDataLen1);
          responseInfo2.Status = num2;
          this.propService.Callback(0, 0, IntPtr.Zero, 0U, ref responseInfo2);
          break;
        case 102:
          uint pDataLen2 = 0;
          ResponseInfo responseInfo3 = new ResponseInfo(0, 0, 0, 0, 0);
          responseInfo1 = PInvokePvicom.PviComGetResponseInfo(this.propService.hPvi, msg.WParam, zero, out pDataLen2, ref responseInfo3, (uint) Marshal.SizeOf(typeof (ResponseInfo)));
          if (pDataLen2 > this.m_LenOfpData)
          {
            if (IntPtr.Zero != this.m_pData)
              PviMarshal.FreeHGlobal(ref this.m_pData);
            this.m_pData = IntPtr.Zero;
            this.m_LenOfpData = pDataLen2;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen2);
          }
          else if (IntPtr.Zero == this.m_pData)
          {
            this.m_LenOfpData = pDataLen2;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen2);
          }
          int num3 = PInvokePvicom.PviComReadResponse(this.propService.hPvi, msg.WParam, this.m_pData, (int) pDataLen2);
          responseInfo3.Status = num3;
          this.propService.Callback(0, 0, IntPtr.Zero, 0U, ref responseInfo3);
          break;
        case 103:
          ResponseInfo responseInfo4 = new ResponseInfo(0, 0, 0, 0, 0);
          responseInfo1 = PInvokePvicom.PviComGetResponseInfo(this.propService.hPvi, msg.WParam, zero, out pDataLen1, ref responseInfo4, (uint) Marshal.SizeOf(typeof (ResponseInfo)));
          if (pDataLen1 > this.m_LenOfpData)
          {
            if (IntPtr.Zero != this.m_pData)
              PviMarshal.FreeHGlobal(ref this.m_pData);
            this.m_pData = IntPtr.Zero;
            this.m_LenOfpData = pDataLen1;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen1);
          }
          else if (IntPtr.Zero == this.m_pData)
          {
            this.m_LenOfpData = pDataLen1;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen1);
          }
          int num4 = PInvokePvicom.PviComReadResponse(this.propService.hPvi, msg.WParam, this.m_pData, (int) pDataLen1);
          responseInfo4.Status = num4;
          this.propService.Callback(0, 0, IntPtr.Zero, 0U, ref responseInfo4);
          break;
        case 111:
        case 114:
        case 116:
        case 118:
        case 120:
        case 150:
        case 212:
        case 216:
        case 269:
        case 291:
        case 415:
        case 505:
        case 613:
        case 614:
        case 615:
        case 616:
        case 617:
        case 621:
        case 622:
        case 624:
        case 700:
        case 702:
        case 721:
        case 722:
        case 725:
        case 726:
        case 727:
        case 728:
        case 729:
        case 917:
        case 1070:
        case 1202:
        case 2812:
          uint pDataLen3 = 0;
          ResponseInfo responseInfo5 = new ResponseInfo(0, 0, 0, 0, 0);
          responseInfo1 = PInvokePvicom.PviComGetResponseInfo(this.propService.hPvi, msg.WParam, zero, out pDataLen3, ref responseInfo5, (uint) Marshal.SizeOf(typeof (ResponseInfo)));
          if (pDataLen3 > this.m_LenOfpData)
          {
            if (IntPtr.Zero != this.m_pData)
              PviMarshal.FreeHGlobal(ref this.m_pData);
            this.m_pData = IntPtr.Zero;
            this.m_LenOfpData = pDataLen3;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen3);
          }
          else if (IntPtr.Zero == this.m_pData)
          {
            this.m_LenOfpData = pDataLen3;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen3);
          }
          int num5 = PInvokePvicom.PviComReadResponse(this.propService.hPvi, msg.WParam, this.m_pData, (int) pDataLen3);
          responseInfo5.Status = num5;
          if (responseInfo5.Mode == 1)
          {
            this.propService.PVICB_Event(iWParam, iLParam, this.m_pData, pDataLen3, ref responseInfo5);
            break;
          }
          this.propService.PVICB_Read(iWParam, iLParam, this.m_pData, pDataLen3, ref responseInfo5);
          break;
        case 112:
        case 113:
        case 115:
        case 117:
        case 119:
        case 121:
        case 123:
        case 124:
        case 151:
        case 701:
        case 704:
        case 706:
        case 708:
          int error2 = PInvokePvicom.PviComLinkResponse(this.propService.hPvi, msg.WParam, out linkID);
          ResponseInfo info1 = new ResponseInfo((int) linkID, 6, 0, error2, 0);
          this.propService.PVICB_Link(iWParam, iLParam, IntPtr.Zero, 0U, ref info1);
          break;
        case 201:
        case 301:
        case 401:
        case 501:
        case 909:
        case 2800:
        case 2803:
        case 2806:
          int response = PInvokePvicom.PviComCreateResponse(this.propService.hPvi, msg.WParam, out linkID);
          ResponseInfo info2 = new ResponseInfo((int) linkID, 4, 0, response, 0);
          this.propService.PVICB_Create(iWParam, iLParam, IntPtr.Zero, 0U, ref info2);
          break;
        case 202:
        case 402:
        case 502:
        case 602:
        case 2801:
        case 2804:
        case 2807:
          ResponseInfo info3 = new ResponseInfo(0, 8, 10, PInvokePvicom.PviComUnlinkResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Unlink(iWParam, iLParam, IntPtr.Zero, 0U, ref info3);
          break;
        case 203:
        case 207:
          ResponseInfo info4 = new ResponseInfo(0, 3, 264, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0U, ref info4);
          break;
        case 204:
          ResponseInfo info5 = new ResponseInfo(0, 3, 263, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0U, ref info5);
          break;
        case 213:
          ResponseInfo info6 = new ResponseInfo(0, 3, 22, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0U, ref info6);
          break;
        case 215:
          ResponseInfo info7 = new ResponseInfo(0, 3, 290, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0U, ref info7);
          break;
        case 219:
          ResponseInfo info8 = new ResponseInfo(0, 3, 29, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0U, ref info8);
          break;
        case 222:
          ResponseInfo info9 = new ResponseInfo(0, 3, 297, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0U, ref info9);
          break;
        case 305:
        case 406:
        case 918:
          ResponseInfo info10 = new ResponseInfo(0, 3, 280, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0U, ref info10);
          break;
        case 307:
          ResponseInfo info11 = new ResponseInfo(0, 3, 21, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0U, ref info11);
          break;
        case 403:
          ResponseInfo info12 = new ResponseInfo(0, 3, 276, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0U, ref info12);
          break;
        case 404:
          ResponseInfo info13 = new ResponseInfo(0, 3, 277, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0U, ref info13);
          break;
        case 503:
          ResponseInfo info14 = new ResponseInfo(0, 3, 5, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_WriteA(iWParam, iLParam, IntPtr.Zero, 0U, ref info14);
          break;
        case 504:
          ResponseInfo info15 = new ResponseInfo(0, 3, 5, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0U, ref info15);
          break;
        case 506:
          ResponseInfo info16 = new ResponseInfo(0, 3, 11, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0U, ref info16);
          break;
        case 512:
          ResponseInfo info17 = new ResponseInfo(0, 3, 15, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0U, ref info17);
          break;
        case 514:
          ResponseInfo info18 = new ResponseInfo(0, 3, 16, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0U, ref info18);
          break;
        case 550:
        case 703:
        case 705:
        case 707:
        case 2802:
        case 2805:
        case 2808:
          uint pDataLen4 = 0;
          ResponseInfo responseInfo6 = new ResponseInfo(0, 0, 0, 0, 0);
          responseInfo1 = PInvokePvicom.PviComGetResponseInfo(this.propService.hPvi, msg.WParam, zero, out pDataLen4, ref responseInfo6, (uint) Marshal.SizeOf(typeof (ResponseInfo)));
          if (pDataLen4 > this.m_LenOfpData)
          {
            if (IntPtr.Zero != this.m_pData)
              PviMarshal.FreeHGlobal(ref this.m_pData);
            this.m_pData = IntPtr.Zero;
            this.m_LenOfpData = pDataLen4;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen4);
          }
          else if (IntPtr.Zero == this.m_pData)
          {
            this.m_LenOfpData = pDataLen4;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen4);
          }
          int num6 = PInvokePvicom.PviComReadResponse(this.propService.hPvi, msg.WParam, this.m_pData, (int) pDataLen4);
          responseInfo6.Status = num6;
          this.propService.PVICB_Event(iWParam, iLParam, this.m_pData, pDataLen4, ref responseInfo6);
          break;
        case 551:
          ResponseInfo info19 = new ResponseInfo(0, 3, 13, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0U, ref info19);
          break;
        case 552:
          ResponseInfo info20 = new ResponseInfo(0, 3, 5, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0U, ref info20);
          break;
        case 618:
          ResponseInfo info21 = new ResponseInfo(0, 3, 281, PInvokePvicom.PviComWriteResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_Write(iWParam, iLParam, IntPtr.Zero, 0U, ref info21);
          break;
        case 709:
          int error3 = PInvokePvicom.PviComLinkResponse(this.propService.hPvi, msg.WParam, out linkID);
          ResponseInfo info22 = new ResponseInfo((int) linkID, 6, 0, error3, 0);
          this.propService.PVICB_LinkA(iWParam, iLParam, IntPtr.Zero, 0U, ref info22);
          break;
        case 710:
          ResponseInfo info23 = new ResponseInfo(0, 8, 0, PInvokePvicom.PviComUnlinkResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_UnlinkA(iWParam, iLParam, IntPtr.Zero, 0U, ref info23);
          break;
        case 711:
          ResponseInfo responseInfo7 = new ResponseInfo(0, 2, 14, error1, 0);
          responseInfo1 = PInvokePvicom.PviComGetResponseInfo(this.propService.hPvi, msg.WParam, zero, out pDataLen1, ref responseInfo7, (uint) Marshal.SizeOf(typeof (ResponseInfo)));
          if (pDataLen1 > this.m_LenOfpData)
          {
            if (IntPtr.Zero != this.m_pData)
              PviMarshal.FreeHGlobal(ref this.m_pData);
            this.m_pData = IntPtr.Zero;
            this.m_LenOfpData = pDataLen1;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen1);
          }
          else if (IntPtr.Zero == this.m_pData)
          {
            this.m_LenOfpData = pDataLen1;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen1);
          }
          int num7 = PInvokePvicom.PviComReadResponse(this.propService.hPvi, msg.WParam, this.m_pData, (int) pDataLen1);
          responseInfo7.Status = num7;
          if (responseInfo7.Mode == 6)
          {
            this.propService.PVICB_LinkA(iWParam, iLParam, this.m_pData, pDataLen1, ref responseInfo7);
            break;
          }
          this.propService.PVICB_EventA(iWParam, iLParam, this.m_pData, pDataLen1, ref responseInfo7);
          break;
        case 916:
          uint pDataLen5 = 0;
          ResponseInfo responseInfo8 = new ResponseInfo(0, 0, 0, 0, 0);
          responseInfo1 = PInvokePvicom.PviComGetResponseInfo(this.propService.hPvi, msg.WParam, zero, out pDataLen5, ref responseInfo8, (uint) Marshal.SizeOf(typeof (ResponseInfo)));
          if (pDataLen5 > this.m_LenOfpData)
          {
            if (IntPtr.Zero != this.m_pData)
              PviMarshal.FreeHGlobal(ref this.m_pData);
            this.m_pData = IntPtr.Zero;
            this.m_LenOfpData = pDataLen5;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen5);
          }
          else if (IntPtr.Zero == this.m_pData)
          {
            this.m_LenOfpData = pDataLen5;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen5);
          }
          int num8 = PInvokePvicom.PviComReadResponse(this.propService.hPvi, msg.WParam, this.m_pData, (int) pDataLen5);
          responseInfo8.Status = num8;
          switch (responseInfo8.Mode)
          {
            case 1:
              this.propService.PVICB_EventA(iWParam, iLParam, this.m_pData, pDataLen5, ref responseInfo8);
              return;
            case 2:
              this.propService.PVICB_Read(iWParam, iLParam, this.m_pData, pDataLen5, ref responseInfo8);
              return;
            case 3:
              this.propService.PVICB_Write(iWParam, iLParam, this.m_pData, pDataLen5, ref responseInfo8);
              return;
            case 4:
              this.propService.PVICB_Create(iWParam, iLParam, this.m_pData, pDataLen5, ref responseInfo8);
              return;
            default:
              return;
          }
        case 2809:
          uint pDataLen6 = 0;
          ResponseInfo responseInfo9 = new ResponseInfo(0, 2, 11, error1, 0);
          responseInfo1 = PInvokePvicom.PviComGetResponseInfo(this.propService.hPvi, msg.WParam, zero, out pDataLen6, ref responseInfo9, (uint) Marshal.SizeOf(typeof (ResponseInfo)));
          if (pDataLen6 > this.m_LenOfpData)
          {
            if (IntPtr.Zero != this.m_pData)
              PviMarshal.FreeHGlobal(ref this.m_pData);
            this.m_pData = IntPtr.Zero;
            this.m_LenOfpData = pDataLen6;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen6);
          }
          else if (IntPtr.Zero == this.m_pData)
          {
            this.m_LenOfpData = pDataLen6;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen6);
          }
          int num9 = PInvokePvicom.PviComReadResponse(this.propService.hPvi, msg.WParam, this.m_pData, (int) pDataLen6);
          responseInfo9.Status = num9;
          this.propService.PVICB_ReadE(iWParam, iLParam, this.m_pData, pDataLen6, ref responseInfo9);
          break;
        case 2810:
          uint pDataLen7 = 0;
          ResponseInfo responseInfo10 = new ResponseInfo(0, 0, 0, 0, 0);
          responseInfo1 = PInvokePvicom.PviComGetResponseInfo(this.propService.hPvi, msg.WParam, zero, out pDataLen7, ref responseInfo10, (uint) Marshal.SizeOf(typeof (ResponseInfo)));
          if (pDataLen7 > this.m_LenOfpData)
          {
            if (IntPtr.Zero != this.m_pData)
              PviMarshal.FreeHGlobal(ref this.m_pData);
            this.m_pData = IntPtr.Zero;
            this.m_LenOfpData = pDataLen7;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen7);
          }
          else if (IntPtr.Zero == this.m_pData)
          {
            this.m_LenOfpData = pDataLen7;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen7);
          }
          int num10 = PInvokePvicom.PviComReadResponse(this.propService.hPvi, msg.WParam, this.m_pData, (int) pDataLen7);
          responseInfo10.Status = num10;
          this.propService.PVICB_ReadA(iWParam, iLParam, this.m_pData, pDataLen7, ref responseInfo10);
          break;
        case 2811:
          uint pDataLen8 = 0;
          ResponseInfo responseInfo11 = new ResponseInfo(0, 0, 0, 0, 0);
          responseInfo1 = PInvokePvicom.PviComGetResponseInfo(this.propService.hPvi, msg.WParam, zero, out pDataLen8, ref responseInfo11, (uint) Marshal.SizeOf(typeof (ResponseInfo)));
          if (pDataLen8 > this.m_LenOfpData)
          {
            if (IntPtr.Zero != this.m_pData)
              PviMarshal.FreeHGlobal(ref this.m_pData);
            this.m_pData = IntPtr.Zero;
            this.m_LenOfpData = pDataLen8;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen8);
          }
          else if (IntPtr.Zero == this.m_pData)
          {
            this.m_LenOfpData = pDataLen8;
            this.m_pData = PviMarshal.AllocHGlobal(pDataLen8);
          }
          int num11 = PInvokePvicom.PviComReadResponse(this.propService.hPvi, msg.WParam, this.m_pData, (int) pDataLen8);
          responseInfo11.Status = num11;
          this.propService.PVICB_ReadE(iWParam, iLParam, this.m_pData, pDataLen8, ref responseInfo11);
          break;
        case 2813:
          ResponseInfo info24 = new ResponseInfo(0, 8, 0, PInvokePvicom.PviComUnlinkResponse(this.propService.hPvi, msg.WParam), 0);
          this.propService.PVICB_UnlinkD(iWParam, iLParam, IntPtr.Zero, 0U, ref info24);
          break;
      }
    }
  }
}
