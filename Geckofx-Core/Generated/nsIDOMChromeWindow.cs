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
// Generated by IDLImporter from file nsIDOMChromeWindow.idl
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
    /// License, v. 2.0. If a copy of the MPL was not distributed with this
    /// file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0c10226f-8abb-4345-aa6b-2780a6f4687e")]
	public interface nsIDOMChromeWindow
	{
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		ushort GetWindowStateAttribute();
		
		/// <summary>
        /// browserDOMWindow provides access to yet another layer of
        /// utility functions implemented by chrome script. It will be null
        /// for DOMWindows not corresponding to browsers.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIBrowserDOMWindow GetBrowserDOMWindowAttribute();
		
		/// <summary>
        /// browserDOMWindow provides access to yet another layer of
        /// utility functions implemented by chrome script. It will be null
        /// for DOMWindows not corresponding to browsers.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetBrowserDOMWindowAttribute([MarshalAs(UnmanagedType.Interface)] nsIBrowserDOMWindow aBrowserDOMWindow);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetAttention();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetAttentionWithCycleCount(int aCycleCount);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetCursor([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase cursor);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Maximize();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Minimize();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Restore();
		
		/// <summary>
        /// Notify a default button is loaded on a dialog or a wizard.
        /// defaultButton is the default button.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void NotifyDefaultButtonLoaded([MarshalAs(UnmanagedType.Interface)] nsIDOMElement defaultButton);
		
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIMessageBroadcaster GetMessageManagerAttribute();
		
		/// <summary>
        /// On some operating systems, we must allow the window manager to
        /// handle window dragging. This function tells the window manager to
        /// start dragging the window. This function will fail unless called
        /// while the left mouse button is held down, callers must check this.
        ///
        /// The optional panel argument should be set when moving a panel.
        ///
        /// Returns NS_ERROR_NOT_IMPLEMENTED (and thus throws in JS) if the OS
        /// doesn't support this.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void BeginWindowMove([MarshalAs(UnmanagedType.Interface)] nsIDOMEvent mouseDownEvent, [MarshalAs(UnmanagedType.Interface)] nsIDOMElement panel);
	}
	
	/// <summary>nsIDOMChromeWindowConsts </summary>
	public class nsIDOMChromeWindowConsts
	{
		
		// <summary>
        //This Source Code Form is subject to the terms of the Mozilla Public
        // License, v. 2.0. If a copy of the MPL was not distributed with this
        // file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
		public const ushort STATE_MAXIMIZED = 1;
		
		// 
		public const ushort STATE_MINIMIZED = 2;
		
		// 
		public const ushort STATE_NORMAL = 3;
		
		// 
		public const ushort STATE_FULLSCREEN = 4;
	}
}
