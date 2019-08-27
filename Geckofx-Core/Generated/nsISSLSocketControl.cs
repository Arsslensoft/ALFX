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
// Generated by IDLImporter from file nsISSLSocketControl.idl
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
    ///-*- Mode: C++; tab-width: 2; indent-tabs-mode: nil; c-basic-offset: 2 -*-
    ///
    /// This Source Code Form is subject to the terms of the Mozilla Public
    /// License, v. 2.0. If a copy of the MPL was not distributed with this
    /// file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("4080f700-9c16-4884-8f8d-e28094377084")]
	public interface nsISSLSocketControl
	{
		
		/// <summary>
        ///-*- Mode: C++; tab-width: 2; indent-tabs-mode: nil; c-basic-offset: 2 -*-
        ///
        /// This Source Code Form is subject to the terms of the Mozilla Public
        /// License, v. 2.0. If a copy of the MPL was not distributed with this
        /// file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIInterfaceRequestor GetNotificationCallbacksAttribute();
		
		/// <summary>
        ///-*- Mode: C++; tab-width: 2; indent-tabs-mode: nil; c-basic-offset: 2 -*-
        ///
        /// This Source Code Form is subject to the terms of the Mozilla Public
        /// License, v. 2.0. If a copy of the MPL was not distributed with this
        /// file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetNotificationCallbacksAttribute([MarshalAs(UnmanagedType.Interface)] nsIInterfaceRequestor aNotificationCallbacks);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ProxyStartSSL();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void StartTLS();
		
		/// <summary>
        ///NPN (Next Protocol Negotiation) is a mechanism for
        ///       negotiating the protocol to be spoken inside the SSL
        ///       tunnel during the SSL handshake. The NPNList is the list
        ///       of offered client side protocols. setNPNList() needs to
        ///       be called before any data is read or written (including the
        ///       handshake to be setup correctly. The server determines the
        ///       priority when multiple matches occur, but if there is no overlap
        ///       the first protocol in the list is used. </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetNPNList(System.IntPtr aNPNList);
		
		/// <summary>
        ///negotiatedNPN is '' if no NPN list was provided by the client,
        /// or if the server did not select any protocol choice from that
        /// list. That also includes the case where the server does not
        /// implement NPN.
        ///
        /// If negotiatedNPN is read before NPN has progressed to the point
        /// where this information is available NS_ERROR_NOT_CONNECTED is
        /// raised.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetNegotiatedNPNAttribute([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aNegotiatedNPN);
		
		/// <summary>
        ///e.g. "spdy/2" </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool JoinConnection([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase npnProtocol, [MarshalAs(UnmanagedType.LPStruct)] nsACStringBase hostname, int port);
		
		/// <summary>
        ///The Key Exchange Algorithm is used when determining whether or
        ///       not to do false start.
        ///       After a handshake is complete it can be read from KEAUsed,
        ///       before a handshake is started it may be set through KEAExpected.
        ///       The values correspond to the SSLKEAType enum in NSS or the
        ///       KEY_EXCHANGE_UNKNOWN constant defined below.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		short GetKEAUsedAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		short GetKEAExpectedAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetKEAExpectedAttribute(short aKEAExpected);
		
		/// <summary>
        /// The original flags from the socket provider.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetProviderFlagsAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		short GetSSLVersionUsedAttribute();
	}
	
	/// <summary>nsISSLSocketControlConsts </summary>
	public class nsISSLSocketControlConsts
	{
		
		// 
		public const short KEY_EXCHANGE_UNKNOWN = -1;
		
		// <summary>
        //These values are defined by TLS. </summary>
		public const short SSL_VERSION_3 = 0x0300;
		
		// 
		public const short TLS_VERSION_1 = 0x0301;
		
		// 
		public const short TLS_VERSION_1_1 = 0x0302;
		
		// 
		public const short TLS_VERSION_1_2 = 0x0303;
		
		// 
		public const short SSL_VERSION_UNKNOWN = -1;
	}
}
