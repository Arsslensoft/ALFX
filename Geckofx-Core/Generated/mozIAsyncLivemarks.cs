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
// Generated by IDLImporter from file mozIAsyncLivemarks.idl
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
	[Guid("5B48E5A2-F07A-4E64-A935-C722A3D60B65")]
	public interface mozIAsyncLivemarks
	{
		
		/// <summary>
        /// Creates a new livemark
        ///
        /// @param aLivemarkInfo
        /// mozILivemarkInfo object containing at least title, parentId,
        /// index and feedURI of the livemark to create.
        /// @param [optional] aCallback
        /// Invoked when the creation process is done.  In case of failure will
        /// receive an error code.
        /// @return {Promise}
        /// @throws NS_ERROR_INVALID_ARG if the supplied information is insufficient
        /// for the creation.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		Gecko.JsVal AddLivemark(Gecko.JsVal aLivemarkInfo, mozILivemarkCallback aCallback);
		
		/// <summary>
        /// Removes an existing livemark.
        ///
        /// @param aLivemarkInfo
        /// mozILivemarkInfo object containing either an id or a guid of the
        /// livemark to remove.
        /// @param [optional] aCallback
        /// Invoked when the removal process is done.  In case of failure will
        /// receive an error code.
        ///
        /// @return {Promise}
        /// @throws NS_ERROR_INVALID_ARG if the id/guid is invalid.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		Gecko.JsVal RemoveLivemark(Gecko.JsVal aLivemarkInfo, mozILivemarkCallback aCallback);
		
		/// <summary>
        /// Gets an existing livemark.
        ///
        /// @param aLivemarkInfo
        /// mozILivemarkInfo object containing either an id or a guid of the
        /// livemark to retrieve.
        /// @param [optional] aCallback
        /// Invoked when the fetching process is done.  In case of failure will
        /// receive an error code.
        ///
        /// @return {Promise}
        /// @throws NS_ERROR_INVALID_ARG if the id/guid is invalid or an invalid
        /// callback is provided.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		Gecko.JsVal GetLivemark(Gecko.JsVal aLivemarkInfo, mozILivemarkCallback aCallback);
		
		/// <summary>
        /// Reloads all livemarks if they are expired or if forced to do so.
        ///
        /// @param [optional]aForceUpdate
        /// If set to true forces a reload even if contents are still valid.
        ///
        /// @note The update process is asynchronous, observers registered through
        /// registerForUpdates will be notified of updated contents.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ReloadLivemarks([MarshalAs(UnmanagedType.U1)] bool aForceUpdate);
	}
	
	/// <summary>mozILivemarkCallback </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("62a426f9-39a6-42f0-ad48-b7404d48188f")]
	public interface mozILivemarkCallback
	{
		
		/// <summary>
        /// Invoked when a livemark is added, removed or retrieved.
        ///
        /// @param aStatus
        /// Whether the request was completed successfully.
        /// Use Components.isSuccessCode(aStatus) to check this.
        /// @param aLivemark
        /// A mozILivemark object representing the livemark, or null on removal.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void OnCompletion(int aStatus, mozILivemark aLivemark);
	}
	
	/// <summary>mozILivemarkInfo </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6e40d5b1-ce48-4458-8b68-6bee17d30ef3")]
	public interface mozILivemarkInfo
	{
		
		/// <summary>
        /// Id of the bookmarks folder representing this livemark.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetIdAttribute();
		
		/// <summary>
        /// The globally unique identifier of this livemark.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetGuidAttribute([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aGuid);
		
		/// <summary>
        /// Title of this livemark.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetTitleAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aTitle);
		
		/// <summary>
        /// Id of the bookmarks parent folder containing this livemark.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetParentIdAttribute();
		
		/// <summary>
        /// The position of this livemark in the bookmarks parent folder.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetIndexAttribute();
		
		/// <summary>
        /// Time this livemark's details were last modified.  Doesn't track changes to
        /// the livemark contents.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetLastModifiedAttribute();
		
		/// <summary>
        /// The URI of the syndication feed associated with this livemark.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIURI GetFeedURIAttribute();
		
		/// <summary>
        /// The URI of the website associated with this livemark.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIURI GetSiteURIAttribute();
	}
	
	/// <summary>mozILivemark </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9f6fdfae-db9a-4bd8-bde1-148758cf1b18")]
	public interface mozILivemark : mozILivemarkInfo
	{
		
		/// <summary>
        /// Id of the bookmarks folder representing this livemark.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new long GetIdAttribute();
		
		/// <summary>
        /// The globally unique identifier of this livemark.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetGuidAttribute([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aGuid);
		
		/// <summary>
        /// Title of this livemark.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetTitleAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aTitle);
		
		/// <summary>
        /// Id of the bookmarks parent folder containing this livemark.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new long GetParentIdAttribute();
		
		/// <summary>
        /// The position of this livemark in the bookmarks parent folder.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetIndexAttribute();
		
		/// <summary>
        /// Time this livemark's details were last modified.  Doesn't track changes to
        /// the livemark contents.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new long GetLastModifiedAttribute();
		
		/// <summary>
        /// The URI of the syndication feed associated with this livemark.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIURI GetFeedURIAttribute();
		
		/// <summary>
        /// The URI of the website associated with this livemark.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsIURI GetSiteURIAttribute();
		
		/// <summary>
        /// Status of this livemark.  One of the STATUS_* constants above.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		ushort GetStatusAttribute();
		
		/// <summary>
        /// Reload livemark contents if they are expired or if forced to do so.
        ///
        /// @param [optional]aForceUpdate
        /// If set to true forces a reload even if contents are still valid.
        ///
        /// @note The update process is asynchronous, it's possible to register a
        /// result observer to be notified of updated contents through
        /// registerForUpdates.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Reload([MarshalAs(UnmanagedType.U1)] bool aForceUpdate);
		
		/// <summary>
        /// Returns an array of nsINavHistoryResultNode objects, representing children
        /// of this livemark.  The nodes will have aContainerNode as parent.
        ///
        /// @param aContainerNode
        /// Object implementing nsINavHistoryContainerResultNode, to be used as
        /// parent of the livemark nodes.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		Gecko.JsVal GetNodesForContainer(Gecko.JsVal aContainerNode);
		
		/// <summary>
        /// Registers a container node for updates on this livemark.
        /// When the livemark contents change, an invalidateContainer(aContainerNode)
        /// request is sent to aResultObserver.
        ///
        /// @param aContainerNode
        /// Object implementing nsINavHistoryContainerResultNode, representing
        /// this livemark.
        /// @param aResultObserver
        /// The nsINavHistoryResultObserver that should be notified of changes
        /// to the livemark contents.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RegisterForUpdates(Gecko.JsVal aContainerNode, [MarshalAs(UnmanagedType.Interface)] nsINavHistoryResultObserver aResultObserver);
		
		/// <summary>
        /// Unregisters a previously registered container node.
        ///
        /// @param aContainerNode
        /// Object implementing nsINavHistoryContainerResultNode, representing
        /// this livemark.
        ///
        /// @note it's suggested to always unregister containers that are no more used,
        /// to free up the associated resources.  A good time to do so is when
        /// the container gets closed.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UnregisterForUpdates(Gecko.JsVal aContainerNode);
	}
	
	/// <summary>mozILivemarkConsts </summary>
	public class mozILivemarkConsts
	{
		
		// <summary>
        // Indicates the livemark is inactive.
        // </summary>
		public const ushort STATUS_READY = 0;
		
		// <summary>
        // Indicates the livemark is fetching new contents.
        // </summary>
		public const ushort STATUS_LOADING = 1;
		
		// <summary>
        // Indicates the livemark failed to fetch new contents.
        // </summary>
		public const ushort STATUS_FAILED = 2;
	}
}
