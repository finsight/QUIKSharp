// =========================================================================
//    Trans2Quik.cs (c) 2013 Nikolay Moroshkin, http://www.moroshkin.com/
// =========================================================================

#region Пример
/* -----------------------------------------------------------------------
  Trans2Quik.Result r;
  int errCode;
  byte[] errMsg = new byte[256];

  r = Trans2Quik.CONNECT(@"F:\Trading\Quik", out errCode, errMsg, errMsg.Length);
  Console.WriteLine(r.ToString() + ", " + errCode + ", " + Trans2Quik.GetString(errMsg));

  r = Trans2Quik.DISCONNECT(out errCode, errMsg, errMsg.Length);
  Console.WriteLine(r.ToString() + ", " + errCode + ", " + Trans2Quik.GetString(errMsg));
----------------------------------------------------------------------- */
#endregion

#region Комментарий
/* ------------------------------------------------------------------------
  В классе реализованы функции-переходники для обеспечения совместимости
  обратных вызовов из неуправляемого кода и сборщика мусора. За счет этого
  задание функций обратных вызовов может осуществляться штатным образом,
  без необходимости их фиксации в куче.

  Результат выполнения функций, которые возвращают IntPtr, необходимо
  преобразовать к string с помощью метода Marshal.PtrToStringAnsi().
------------------------------------------------------------------------ */
#endregion

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Trans2QuikAPI
{
  static class Trans2Quik
  {
    #region Тип возвращаемых значений

    // **********************************************************************

    public enum Result : int
    {
      SUCCESS = 0,
      FAILED = 1,
      QUIK_TERMINAL_NOT_FOUND = 2,
      DLL_VERSION_NOT_SUPPORTED = 3,
      ALREADY_CONNECTED_TO_QUIK = 4,
      WRONG_SYNTAX = 5,
      QUIK_NOT_CONNECTED = 6,
      DLL_NOT_CONNECTED = 7,
      QUIK_CONNECTED = 8,
      QUIK_DISCONNECTED = 9,
      DLL_CONNECTED = 10,
      DLL_DISCONNECTED = 11,
      MEMORY_ALLOCATION_ERROR = 12,
      WRONG_CONNECTION_HANDLE = 13,
      WRONG_INPUT_PARAMS = 14
    }

    // **********************************************************************

    #endregion

    #region Преобразование строки

    // **********************************************************************

    static readonly Encoding win1251 = Encoding.GetEncoding(1251);

    public static string GetString(byte[] lpstr)
    {
      int count = 0;

      while(count < lpstr.Length && lpstr[count] != 0)
        count++;

      return win1251.GetString(lpstr, 0, count);
    }

    // **********************************************************************

    #endregion

    #region Функции для работы с транзакциями

    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_SEND_SYNC_TRANSACTION (
    //   LPSTR lpstTransactionString,
    //   long* pnReplyCode,
    //   PDWORD pdwTransId,
    //   double* pdOrderNum,
    //   LPSTR lpstrResultMessage,
    //   DWORD dwResultMessageSize,
    //   long* pnExtendedErrorCode,
    //   LPSTR lpstErrorMessage,
    //   DWORD dwErrorMessageSize);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_SEND_SYNC_TRANSACTION@36",
      CallingConvention = CallingConvention.StdCall)]
    public static extern Result SEND_SYNC_TRANSACTION(
      [MarshalAs(UnmanagedType.LPStr)] string lpstTransactionString,
      out int pnReplyCode,
      out int pdwTransId,
      out double pdOrderNum,
      byte[] lpstrResultMessage,
      int dwResultMessageSize,
      out int pnExtendedErrorCode,
      byte[] lpstErrorMessage,
      int dwErrorMessageSize);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_SEND_ASYNC_TRANSACTION (
    //   LPSTR lpstTransactionString,
    //   long* pnExtendedErrorCode,
    //   LPSTR lpstErrorMessage,
    //   DWORD dwErrorMessageSize);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_SEND_ASYNC_TRANSACTION@16",
      CallingConvention = CallingConvention.StdCall)]
    public static extern Result SEND_ASYNC_TRANSACTION(
      [MarshalAs(UnmanagedType.LPStr)] string lpstTransactionString,
      out int pnExtendedErrorCode,
      byte[] lpstErrorMessage,
      int dwErrorMessageSize);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_CONNECT (
    //   LPSTR lpstConnectionParamsString,
    //   long* pnExtendedErrorCode,
    //   LPSTR lpstrErrorMessage,
    //   DWORD dwErrorMessageSize);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_CONNECT@16",
      CallingConvention = CallingConvention.StdCall)]
    public static extern Result CONNECT(
      [MarshalAs(UnmanagedType.LPStr)] string lpstConnectionParamsString,
      out int pnExtendedErrorCode,
      byte[] lpstrErrorMessage,
      int dwErrorMessageSize);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_DISCONNECT (
    //   long* pnExtendedErrorCode,
    //   LPSTR lpstrErrorMessage,
    //   DWORD dwErrorMessageSize);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_DISCONNECT@12",
      CallingConvention = CallingConvention.StdCall)]
    public static extern Result DISCONNECT(
      out int pnExtendedErrorCode,
      byte[] lpstrErrorMessage,
      int dwErrorMessageSize);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_IS_QUIK_CONNECTED (
    //   long* pnExtendedErrorCode,
    //   LPSTR lpstrErrorMessage,
    //   DWORD dwErrorMessageSize);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_IS_QUIK_CONNECTED@12",
      CallingConvention = CallingConvention.StdCall)]
    public static extern Result IS_QUIK_CONNECTED(
      out int pnExtendedErrorCode,
      byte[] lpstrErrorMessage,
      int dwErrorMessageSize);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_IS_DLL_CONNECTED (
    //   long* pnExtendedErrorCode,
    //   LPSTR lpstrErrorMessage,
    //   DWORD dwErrorMessageSize);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_IS_DLL_CONNECTED@12",
      CallingConvention = CallingConvention.StdCall)]
    public static extern Result IS_DLL_CONNECTED(
      out int pnExtendedErrorCode,
      byte[] lpstrErrorMessage,
      int dwErrorMessageSize);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_SUBSCRIBE_ORDERS (
    //   LPSTR ClassCode,
    //   LPSTR Seccodes);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_SUBSCRIBE_ORDERS@8",
      CallingConvention = CallingConvention.StdCall)]
    public static extern Result SUBSCRIBE_ORDERS(
      [MarshalAs(UnmanagedType.LPStr)] string ClassCode,
      [MarshalAs(UnmanagedType.LPStr)] string Seccodes);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_UNSUBSCRIBE_ORDERS ();

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_UNSUBSCRIBE_ORDERS@0",
      CallingConvention = CallingConvention.StdCall)]
    public static extern Result UNSUBSCRIBE_ORDERS();


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_SUBSCRIBE_TRADES (
    //   LPSTR ClassCode,
    //   LPSTR Seccodes);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_SUBSCRIBE_TRADES@8",
      CallingConvention = CallingConvention.StdCall)]
    public static extern Result SUBSCRIBE_TRADES(
      [MarshalAs(UnmanagedType.LPStr)] string ClassCode,
      [MarshalAs(UnmanagedType.LPStr)] string Seccodes);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_UNSUBSCRIBE_TRADES ();

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_UNSUBSCRIBE_TRADES@0",
      CallingConvention = CallingConvention.StdCall)]
    public static extern Result UNSUBSCRIBE_TRADES();

    #endregion

    #region Orders

    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_ORDER_QTY (
    //   long nOrderDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_ORDER_QTY@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern int ORDER_QTY(
      int nOrderDescriptor);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_ORDER_DATE (
    //   long nOrderDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_ORDER_DATE@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern int ORDER_DATE(
      int nOrderDescriptor);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_ORDER_TIME (
    //   long nOrderDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_ORDER_TIME@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern int ORDER_TIME(
      int nOrderDescriptor);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_ORDER_ACTIVATION_TIME (
    //   long nOrderDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_ORDER_ACTIVATION_TIME@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern int ORDER_ACTIVATION_TIME(
      int nOrderDescriptor);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_ORDER_WITHDRAW_TIME (
    //   long nOrderDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_ORDER_WITHDRAW_TIME@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern int ORDER_WITHDRAW_TIME(
      int nOrderDescriptor);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_ORDER_EXPIRY (
    //   long nOrderDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_ORDER_EXPIRY@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern int ORDER_EXPIRY(
      int nOrderDescriptor);


    // **********************************************************************
    // double TRANS2QUIK_API __stdcall TRANS2QUIK_ORDER_ACCRUED_INT (
    //   long nOrderDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_ORDER_ACCRUED_INT@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern double ORDER_ACCRUED_INT(
      int nOrderDescriptor);


    // **********************************************************************
    // double TRANS2QUIK_API __stdcall TRANS2QUIK_ORDER_YIELD (
    //   long nOrderDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_ORDER_YIELD@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern double ORDER_YIELD(
      int nOrderDescriptor);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_ORDER_UID (
    //   long nOrderDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_ORDER_UID@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern int ORDER_UID(
      int nOrderDescriptor);


    // **********************************************************************
    // LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_ORDER_USERID (
    //   long nOrderDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_ORDER_USERID@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr ORDER_USERID(
      int nOrderDescriptor);


    // **********************************************************************
    // LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_ORDER_ACCOUNT (
    //   long nOrderDescriptor); 

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_ORDER_ACCOUNT@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr ORDER_ACCOUNT(
      int nOrderDescriptor);


    // **********************************************************************
    // LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_ORDER_BROKERREF (
    //   long nOrderDescriptor); 

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_ORDER_BROKERREF@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr ORDER_BROKERREF(
      int nOrderDescriptor);


    // **********************************************************************
    // LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_ORDER_CLIENT_CODE (
    //   long nOrderDescriptor); 

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_ORDER_CLIENT_CODE@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr ORDER_CLIENT_CODE(
      int nOrderDescriptor);


    // **********************************************************************
    // LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_ORDER_FIRMID (
    //   long nOrderDescriptor); 

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_ORDER_FIRMID@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr ORDER_FIRMID(
      int nOrderDescriptor);

    #endregion

    #region Trades

    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_DATE (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_DATE@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern int TRADE_DATE(
      int nTradeDescriptor);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_SETTLE_DATE (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_SETTLE_DATE@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern int TRADE_SETTLE_DATE(
      int nTradeDescriptor);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_TIME (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_TIME@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern int TRADE_TIME(
      int nTradeDescriptor);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_IS_MARGINAL (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_IS_MARGINAL@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern int TRADE_IS_MARGINAL(
      int nTradeDescriptor);


    // **********************************************************************
    // double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_ACCRUED_INT (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_ACCRUED_INT@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern double TRADE_ACCRUED_INT(
      int nTradeDescriptor);


    // **********************************************************************
    // double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_YIELD (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_YIELD@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern double TRADE_YIELD(
      int nTradeDescriptor);


    // **********************************************************************
    // double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_TS_COMMISSION (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_TS_COMMISSION@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern double TRADE_TS_COMMISSION(
      int nTradeDescriptor);


    // **********************************************************************
    // double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_CLEARING_CENTER_COMMISSION (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_CLEARING_CENTER_COMMISSION@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern double TRADE_CLEARING_CENTER_COMMISSION(
      int nTradeDescriptor);


    // **********************************************************************
    // double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_EXCHANGE_COMMISSION (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_EXCHANGE_COMMISSION@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern double TRADE_EXCHANGE_COMMISSION(
      int nTradeDescriptor);


    // **********************************************************************
    // double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_TRADING_SYSTEM_COMMISSION (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_TRADING_SYSTEM_COMMISSION@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern double TRADE_TRADING_SYSTEM_COMMISSION(
      int nTradeDescriptor);


    // **********************************************************************
    // double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_PRICE2 (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_PRICE2@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern double TRADE_PRICE2(
      int nTradeDescriptor);


    // **********************************************************************
    // double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_REPO_RATE (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_REPO_RATE@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern double TRADE_REPO_RATE(
      int nTradeDescriptor);


    // **********************************************************************
    // double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_REPO_VALUE (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_REPO_VALUE@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern double TRADE_REPO_VALUE(
      int nTradeDescriptor);


    // **********************************************************************
    // double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_REPO2_VALUE (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_REPO2_VALUE@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern double TRADE_REPO2_VALUE(
      int nTradeDescriptor);


    // **********************************************************************
    // double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_ACCRUED_INT2 (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_ACCRUED_INT2@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern double TRADE_ACCRUED_INT2(
      int nTradeDescriptor);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_REPO_TERM (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_REPO_TERM@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern int TRADE_REPO_TERM(
      int nTradeDescriptor);


    // **********************************************************************
    // double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_START_DISCOUNT (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_START_DISCOUNT@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern double TRADE_START_DISCOUNT(
      int nTradeDescriptor);


    // **********************************************************************
    // double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_LOWER_DISCOUNT (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_LOWER_DISCOUNT@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern double TRADE_LOWER_DISCOUNT(
      int nTradeDescriptor);


    // **********************************************************************
    // double TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_UPPER_DISCOUNT (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_UPPER_DISCOUNT@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern double TRADE_UPPER_DISCOUNT(
      int nTradeDescriptor);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_BLOCK_SECURITIES (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_BLOCK_SECURITIES@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern int TRADE_BLOCK_SECURITIES(
      int nTradeDescriptor);


    // **********************************************************************
    // LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_CURRENCY (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_CURRENCY@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr TRADE_CURRENCY(
      int nTradeDescriptor);


    // **********************************************************************
    // LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_SETTLE_CURRENCY (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_SETTLE_CURRENCY@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr TRADE_SETTLE_CURRENCY(
      int nTradeDescriptor);


    // **********************************************************************
    // LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_SETTLE_CODE (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_SETTLE_CODE@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr TRADE_SETTLE_CODE(
      int nTradeDescriptor);


    // **********************************************************************
    // LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_ACCOUNT (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_ACCOUNT@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr TRADE_ACCOUNT(
      int nTradeDescriptor);


    // **********************************************************************
    // LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_BROKERREF (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_BROKERREF@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr TRADE_BROKERREF(
      int nTradeDescriptor);


    // **********************************************************************
    // LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_CLIENT_CODE (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_CLIENT_CODE@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr TRADE_CLIENT_CODE(
      int nTradeDescriptor);


    // **********************************************************************
    // LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_USERID (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_USERID@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr TRADE_USERID(
      int nTradeDescriptor);


    // **********************************************************************
    // LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_FIRMID (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_FIRMID@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr TRADE_FIRMID(
      int nTradeDescriptor);


    // **********************************************************************
    // LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_PARTNER_FIRMID (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_PARTNER_FIRMID@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr TRADE_PARTNER_FIRMID(
      int nTradeDescriptor);


    // **********************************************************************
    // LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_EXCHANGE_CODE (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_EXCHANGE_CODE@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr TRADE_EXCHANGE_CODE(
      int nTradeDescriptor);


    // **********************************************************************
    // LPTSTR TRANS2QUIK_API __stdcall TRANS2QUIK_TRADE_STATION_ID (
    //   long nTradeDescriptor);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_TRADE_STATION_ID@4",
      CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr TRADE_STATION_ID(
      int nTradeDescriptor);

    #endregion

    #region Приватные делегаты функций обратных вызовов

    // **********************************************************************
    // typedef void (__stdcall *TRANS2QUIK_CONNECTION_STATUS_CALLBACK) (
    //   long nConnectionEvent,
    //   long nExtendedErrorCode,
    //   LPCSTR lpcstrInfoMessage);

    delegate void CONNECTION_STATUS_CALLBACK_UNMGR(
      Result nConnectionEvent,
      int nExtendedErrorCode,
      IntPtr lpcstrInfoMessage);


    // **********************************************************************
    // typedef void (__stdcall *TRANS2QUIK_TRANSACTION_REPLY_CALLBACK) (
    //   long nTransactionResult,
    //   long nTransactionExtendedErrorCode,
    //   long nTransactionReplyCode,
    //   DWORD dwTransId,
    //   double dOrderNum,
    //   LPCSTR lpcstrTransactionReplyMessage);

    delegate void TRANSACTION_REPLY_CALLBACK_UNMGR(
      Result nTransactionResult,
      int nTransactionExtendedErrorCode,
      int nTransactionReplyCode,
      int dwTransId,
      double dOrderNum,
      IntPtr lpcstrTransactionReplyMessage);


    // **********************************************************************
    // typedef void (__stdcall *TRANS2QUIK_ORDER_STATUS_CALLBACK) (
    //   long nMode,
    //   DWORD dwTransID,
    //   double dNumber,
    //   LPCSTR ClassCode,
    //   LPCSTR SecCode,
    //   double dPrice,
    //   long nBalance,
    //   double dValue,
    //   long nIsSell,
    //   long nStatus,
    //   long nOrderDescriptor);

    delegate void ORDER_STATUS_CALLBACK_UNMGR(
      int nMode,
      int dwTransID,
      double dNumber,
      IntPtr ClassCode,
      IntPtr SecCode,
      double dPrice,
      int nBalance,
      double dValue,
      int nIsSell,
      int nStatus,
      int nOrderDescriptor);


    // **********************************************************************
    // typedef void (__stdcall *TRANS2QUIK_TRADE_STATUS_CALLBACK) (
    //   long nMode,
    //   double dNumber,
    //   double dOrderNumber,
    //   LPCSTR ClassCode,
    //   LPCSTR SecCode,
    //   double dPrice,
    //   long nQty,
    //   double dValue,
    //   long nIsSell,
    //   long nTradeDescriptor);

    delegate void TRADE_STATUS_CALLBACK_UNMGR(
      int nMode,
      double dNumber,
      double dOrderNumber,
      IntPtr ClassCode,
      IntPtr SecCode,
      double dPrice,
      int nQty,
      double dValue,
      int nIsSell,
      int nTradeDescriptor);

    #endregion

    #region Приватные функции установки обратных вызовов

    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK (
    //   TRANS2QUIK_CONNECTION_STATUS_CALLBACK pfConnectionStatusCallback,
    //   long* pnExtendedErrorCode,
    //   LPSTR lpstrErrorMessage,
    //   DWORD dwErrorMessageSize);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_SET_CONNECTION_STATUS_CALLBACK@16",
      CallingConvention = CallingConvention.StdCall)]
    static extern Result SET_CONNECTION_STATUS_CALLBACK_UNMGR(
      CONNECTION_STATUS_CALLBACK_UNMGR pfConnectionStatusCallback,
      out int pnExtendedErrorCode,
      byte[] lpstrErrorMessage,
      int dwErrorMessageSize);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_SET_TRANSACTIONS_REPLY_CALLBACK (
    //   TRANS2QUIK_TRANSACTION_REPLY_CALLBACK pfTransactionReplyCallback,
    //   long* pnExtendedErrorCode,
    //   LPSTR lpstrErrorMessage,
    //   DWORD dwErrorMessageSize);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_SET_TRANSACTIONS_REPLY_CALLBACK@16",
      CallingConvention = CallingConvention.StdCall)]
    static extern Result SET_TRANSACTIONS_REPLY_CALLBACK_UNMGR(
      TRANSACTION_REPLY_CALLBACK_UNMGR pfTransactionReplyCallback,
      out int pnExtendedErrorCode,
      byte[] lpstrErrorMessage,
      int dwErrorMessageSize);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_START_TRADES(
    //   TRANS2QUIK_TRADE_STATUS_CALLBACK pfnTradeStatusCallback);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_START_TRADES@4",
      CallingConvention = CallingConvention.StdCall)]
    static extern Result START_TRADES_UNMGR(
      TRADE_STATUS_CALLBACK_UNMGR pfnTradeStatusCallback);


    // **********************************************************************
    // long TRANS2QUIK_API __stdcall TRANS2QUIK_START_ORDERS (
    //   TRANS2QUIK_ORDER_STATUS_CALLBACK pfnOrderStatusCallback);

    [DllImport("TRANS2QUIK.DLL", EntryPoint = "_TRANS2QUIK_START_ORDERS@4",
      CallingConvention = CallingConvention.StdCall)]
    static extern Result START_ORDERS_UNMGR(
      ORDER_STATUS_CALLBACK_UNMGR pfnOrderStatusCallback);

    #endregion

    #region Делегаты для управляемого кода

    // **********************************************************************

    public delegate void CONNECTION_STATUS_CALLBACK(
      Result nConnectionEvent,
      int nExtendedErrorCode,
      string lpcstrInfoMessage);

    // **********************************************************************

    public delegate void TRANSACTION_REPLY_CALLBACK(
      Result nTransactionResult,
      int nTransactionExtendedErrorCode,
      int nTransactionReplyCode,
      int dwTransId,
      double dOrderNum,
      string lpcstrTransactionReplyMessage);

    // **********************************************************************

    public delegate void ORDER_STATUS_CALLBACK(
      int nMode,
      int dwTransID,
      double dNumber,
      string ClassCode,
      string SecCode,
      double dPrice,
      int nBalance,
      double dValue,
      int nIsSell,
      int nStatus,
      int nOrderDescriptor);

    // **********************************************************************

    public delegate void TRADE_STATUS_CALLBACK(
      int nMode,
      double dNumber,
      double dOrderNumber,
      string ClassCode,
      string SecCode,
      double dPrice,
      int nQty,
      double dValue,
      int nIsSell,
      int nTradeDescriptor);

    #endregion

    #region Публичные функции установки обратных вызовов

    // **********************************************************************

    public static Result SET_CONNECTION_STATUS_CALLBACK(
      CONNECTION_STATUS_CALLBACK pfConnectionStatusCallback,
      out int pnExtendedErrorCode,
      byte[] lpstrErrorMessage,
      int dwErrorMessageSize)
    {
      connection_status_callback = pfConnectionStatusCallback;
      return SET_CONNECTION_STATUS_CALLBACK_UNMGR(
        connection_status_callback_unmgr,
        out pnExtendedErrorCode,
        lpstrErrorMessage,
        lpstrErrorMessage.Length);
    }

    // **********************************************************************

    public static Result SET_TRANSACTIONS_REPLY_CALLBACK(
      TRANSACTION_REPLY_CALLBACK pfTransactionReplyCallback,
      out int pnExtendedErrorCode,
      byte[] lpstrErrorMessage,
      int dwErrorMessageSize)
    {
      transaction_reply_callback = pfTransactionReplyCallback;
      return SET_TRANSACTIONS_REPLY_CALLBACK_UNMGR(
        transaction_reply_callback_unmgr,
        out pnExtendedErrorCode,
        lpstrErrorMessage,
        lpstrErrorMessage.Length);
    }

    // **********************************************************************

    public static Result START_ORDERS(
      ORDER_STATUS_CALLBACK pfnOrderStatusCallback)
    {
      order_status_callback = pfnOrderStatusCallback;
      return START_ORDERS_UNMGR(
        order_status_callback_unmgr);
    }

    // **********************************************************************

    public static Result START_TRADES(
      TRADE_STATUS_CALLBACK pfnTradeStatusCallback)
    {
      trade_status_callback = pfnTradeStatusCallback;
      return START_TRADES_UNMGR(
        trade_status_callback_unmgr);
    }

    #endregion

    #region Реализация функций-переходников обратных вызовов

    // **********************************************************************

    static void connection_status_callback_impl(
      Result nConnectionEvent,
      int nExtendedErrorCode,
      IntPtr lpcstrInfoMessage)
    {
      if(connection_status_callback != null)
        connection_status_callback(
          nConnectionEvent,
          nExtendedErrorCode,
          Marshal.PtrToStringAnsi(lpcstrInfoMessage));
    }

    // **********************************************************************

    static void transaction_reply_callback_impl(
      Result nTransactionResult,
      int nTransactionExtendedErrorCode,
      int nTransactionReplyCode,
      int dwTransId,
      double dOrderNum,
      IntPtr lpcstrTransactionReplyMessage)
    {
      if(transaction_reply_callback != null)
        transaction_reply_callback(
          nTransactionResult,
          nTransactionExtendedErrorCode,
          nTransactionReplyCode,
          dwTransId,
          dOrderNum,
          Marshal.PtrToStringAnsi(lpcstrTransactionReplyMessage));
    }

    // **********************************************************************

    static void order_status_callback_impl(
      int nMode,
      int dwTransID,
      double dNumber,
      IntPtr ClassCode,
      IntPtr SecCode,
      double dPrice,
      int nBalance,
      double dValue,
      int nIsSell,
      int nStatus,
      int nOrderDescriptor)
    {
      if(order_status_callback != null)
        order_status_callback(
          nMode,
          dwTransID,
          dNumber,
          Marshal.PtrToStringAnsi(ClassCode),
          Marshal.PtrToStringAnsi(SecCode),
          dPrice,
          nBalance,
          dValue,
          nIsSell,
          nStatus,
          nOrderDescriptor);
    }

    // **********************************************************************

    static void trade_status_callback_impl(
      int nMode,
      double dNumber,
      double dOrderNumber,
      IntPtr ClassCode,
      IntPtr SecCode,
      double dPrice,
      int nQty,
      double dValue,
      int nIsSell,
      int nTradeDescriptor)
    {
      if(trade_status_callback != null)
        trade_status_callback(
          nMode,
          dNumber,
          dOrderNumber,
          Marshal.PtrToStringAnsi(ClassCode),
          Marshal.PtrToStringAnsi(SecCode),
          dPrice,
          nQty,
          dValue,
          nIsSell,
          nTradeDescriptor);
    }

    #endregion

    #region Рабочие переменные и конструктор

    // **********************************************************************

    static CONNECTION_STATUS_CALLBACK connection_status_callback;
    static TRANSACTION_REPLY_CALLBACK transaction_reply_callback;
    static ORDER_STATUS_CALLBACK order_status_callback;
    static TRADE_STATUS_CALLBACK trade_status_callback;

    // **********************************************************************

    static CONNECTION_STATUS_CALLBACK_UNMGR connection_status_callback_unmgr =
      new CONNECTION_STATUS_CALLBACK_UNMGR(connection_status_callback_impl);

    static TRANSACTION_REPLY_CALLBACK_UNMGR transaction_reply_callback_unmgr =
      new TRANSACTION_REPLY_CALLBACK_UNMGR(transaction_reply_callback_impl);

    static ORDER_STATUS_CALLBACK_UNMGR order_status_callback_unmgr =
      new ORDER_STATUS_CALLBACK_UNMGR(order_status_callback_impl);

    static TRADE_STATUS_CALLBACK_UNMGR trade_status_callback_unmgr =
      new TRADE_STATUS_CALLBACK_UNMGR(trade_status_callback_impl);

    // **********************************************************************

    static GCHandle gc_connection_status = GCHandle.Alloc(connection_status_callback_unmgr);
    static GCHandle gc_transaction_reply = GCHandle.Alloc(transaction_reply_callback_unmgr);
    static GCHandle gc_order_status = GCHandle.Alloc(order_status_callback_unmgr);
    static GCHandle gc_trade_status = GCHandle.Alloc(trade_status_callback_unmgr);

    // **********************************************************************

    #endregion
  }
}
