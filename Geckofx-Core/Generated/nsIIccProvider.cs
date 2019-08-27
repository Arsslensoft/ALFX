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
// Generated by IDLImporter from file nsIIccProvider.idl
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
	[Guid("7c0ada3d-d8d4-493e-9243-fa3df39855e4")]
	public interface nsIIccListener
	{
		
		/// <summary>
        ///This Source Code Form is subject to the terms of the Mozilla Public
        /// License, v. 2.0. If a copy of the MPL was not distributed with this file,
        /// You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyStkCommand([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aMessage);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyStkSessionEnd();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyCardStateChanged();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyIccInfoChanged();
	}
	
	/// <summary>
    /// XPCOM component (in the content process) that provides the ICC information.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7c67ab92-52a3-4e11-995c-c0ad2f66c4cb")]
	public interface nsIIccProvider
	{
		
		/// <summary>
        /// Called when a content process registers receiving unsolicited messages from
        /// RadioInterfaceLayer in the chrome process. Only a content process that has
        /// the 'mobileconnection' permission is allowed to register.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RegisterIccMsg(uint clientId, [MarshalAs(UnmanagedType.Interface)] nsIIccListener listener);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UnregisterIccMsg(uint clientId, [MarshalAs(UnmanagedType.Interface)] nsIIccListener listener);
		
		/// <summary>
        /// UICC Information
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMMozIccInfo GetIccInfo(uint clientId);
		
		/// <summary>
        /// Card State
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetCardState(uint clientId, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase retval);
		
		/// <summary>
        /// STK interfaces.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SendStkResponse(uint clientId, [MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, Gecko.JsVal command, Gecko.JsVal response);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SendStkMenuSelection(uint clientId, [MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, ushort itemIdentifier, [MarshalAs(UnmanagedType.U1)] bool helpRequested);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SendStkTimerExpiration(uint clientId, [MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, Gecko.JsVal timer);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SendStkEventDownload(uint clientId, [MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, Gecko.JsVal @event);
		
		/// <summary>
        /// Card lock interfaces.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest GetCardLockState(uint clientId, [MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase lockType);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest UnlockCardLock(uint clientId, [MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, Gecko.JsVal info);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest SetCardLock(uint clientId, [MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, Gecko.JsVal info);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest GetCardLockRetryCount(uint clientId, [MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase lockType);
		
		/// <summary>
        /// Phonebook interfaces.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest ReadContacts(uint clientId, [MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase contactType);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest UpdateContact(uint clientId, [MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase contactType, Gecko.JsVal contact, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase pin2);
		
		/// <summary>
        /// Secure Card Icc communication channel
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest IccOpenChannel(uint clientId, [MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aid);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest IccExchangeAPDU(uint clientId, [MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, int channel, Gecko.JsVal apdu);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest IccCloseChannel(uint clientId, [MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, int channel);
		
		/// <summary>
        /// Helpers
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest MatchMvno(uint clientId, [MarshalAs(UnmanagedType.Interface)] nsIDOMWindow window, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase mvnoType, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase mvnoData);
	}
}
