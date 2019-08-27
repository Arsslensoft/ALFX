// --------------------------------------------------------------------------------------------
// Version: MPL 1.1/GPL 2.0/LGPL 2.1
// 
// The contents of this file are subject to the Mozilla Public License Version
// 1.1 (the "License"); you may not use this file except in compliance with
// the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
// for the specific language governing rights and limitations under the
// License.
// 
// <remarks>
// Generated by IDLImporter from file nsIUDPSocketChild.idl
// 
// You should use these interfaces when you access the COM objects defined in the mentioned
// IDL/IDH file.
// </remarks>
// --------------------------------------------------------------------------------------------
namespace Gecko
{
	using System;
	using System.Runtime.InteropServices;
	using System.Runtime.InteropServices.ComTypes;
	using System.Runtime.CompilerServices;
	
	
	/// <summary>
    ///This Source Code Form is subject to the terms of the Mozilla Public
    /// License, v. 2.0. If a copy of the MPL was not distributed with this file,
    /// You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B47E5A0F-D384-48EF-8885-4259793D9CF0")]
	public interface nsIUDPSocketChild
	{
		
		/// <summary>
        ///This Source Code Form is subject to the terms of the Mozilla Public
        /// License, v. 2.0. If a copy of the MPL was not distributed with this file,
        /// You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		ushort GetLocalPortAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetLocalAddressAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase aLocalAddress);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetFilterNameAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase aFilterName);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetFilterNameAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase aFilterName);
		
		/// <summary>
        /// Tell the chrome process to bind the UDP socket to a given local host and port
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Bind([MarshalAs(UnmanagedType.Interface)] nsIUDPSocketInternal socket, [MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase host, ushort port);
		
		/// <summary>
        /// Tell the chrome process to perform equivalent operations to all following methods
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Send([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase host, ushort port, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=3)] byte[] bytes, uint byteLength);
		
		/// <summary>
        /// Send without DNS query
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SendWithAddr([MarshalAs(UnmanagedType.Interface)] nsINetAddr addr, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] byte[] bytes, uint byteLength);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SendWithAddress(System.IntPtr addr, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=2)] byte[] bytes, uint byteLength);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Close();
	}
	
	/// <summary>
    /// Internal interface for callback from chrome process
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("1E27E9B3-C1C8-4B05-A415-1A2C1A641C60")]
	public interface nsIUDPSocketInternal
	{
		
		/// <summary>
        /// Internal interface for callback from chrome process
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CallListenerError([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase type, [MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase message, [MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase filename, uint lineNumber, uint columnNumber);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CallListenerReceivedData([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase type, [MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase host, ushort port, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex=4)] byte[] data, uint dataLength);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CallListenerVoid([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase type);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CallListenerSent([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase type, int status);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UpdateReadyState([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase readyState);
	}
}