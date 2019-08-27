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
// Generated by IDLImporter from file nsIOfflineCacheUpdate.idl
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
	[Guid("47360d57-8ef4-4a5d-8865-1a27a739ad1a")]
	public interface nsIOfflineCacheUpdateObserver
	{
		
		/// <summary>
        /// aUpdate has changed its state.
        ///
        /// @param aUpdate
        /// The nsIOfflineCacheUpdate being processed.
        /// @param event
        /// See enumeration above
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UpdateStateChanged([MarshalAs(UnmanagedType.Interface)] nsIOfflineCacheUpdate aUpdate, uint state);
		
		/// <summary>
        /// Informs the observer about an application being available to associate.
        ///
        /// @param applicationCache
        /// The application cache instance that has been created or found by the
        /// update to associate with
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ApplicationCacheAvailable([MarshalAs(UnmanagedType.Interface)] nsIApplicationCache applicationCache);
	}
	
	/// <summary>nsIOfflineCacheUpdateObserverConsts </summary>
	public class nsIOfflineCacheUpdateObserverConsts
	{
		
		// <summary>
        //This Source Code Form is subject to the terms of the Mozilla Public
        // License, v. 2.0. If a copy of the MPL was not distributed with this
        // file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
		public const ulong STATE_ERROR = 1;
		
		// 
		public const ulong STATE_CHECKING = 2;
		
		// 
		public const ulong STATE_NOUPDATE = 3;
		
		// 
		public const ulong STATE_OBSOLETE = 4;
		
		// 
		public const ulong STATE_DOWNLOADING = 5;
		
		// 
		public const ulong STATE_ITEMSTARTED = 6;
		
		// 
		public const ulong STATE_ITEMCOMPLETED = 7;
		
		// 
		public const ulong STATE_ITEMPROGRESS = 8;
		
		// 
		public const ulong STATE_FINISHED = 10;
	}
	
	/// <summary>
    /// An nsIOfflineCacheUpdate is used to update an application's offline
    /// resources.
    ///
    /// It can be used to perform partial or complete updates.
    ///
    /// One update object will be updating at a time.  The active object will
    /// load its items one by one, sending itemCompleted() to any registered
    /// observers.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a4503a53-6ab8-4b50-b01e-1c4f393fc980")]
	public interface nsIOfflineCacheUpdate
	{
		
		/// <summary>
        /// Fetch the status of the running update.  This will return a value
        /// defined in nsIDOMOfflineResourceList.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		ushort GetStatusAttribute();
		
		/// <summary>
        /// TRUE if the update is being used to add specific resources.
        /// FALSE if the complete cache update process is happening.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetPartialAttribute();
		
		/// <summary>
        /// TRUE if this is an upgrade attempt, FALSE if it is a new cache
        /// attempt.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetIsUpgradeAttribute();
		
		/// <summary>
        /// The domain being updated, and the domain that will own any URIs added
        /// with this update.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetUpdateDomainAttribute([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aUpdateDomain);
		
		/// <summary>
        /// The manifest for the offline application being updated.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIURI GetManifestURIAttribute();
		
		/// <summary>
        /// TRUE if the cache update completed successfully.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetSucceededAttribute();
		
		/// <summary>
        /// Initialize the update.
        ///
        /// @param aManifestURI
        /// The manifest URI to be checked.
        /// @param aDocumentURI
        /// The page that is requesting the update.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Init([MarshalAs(UnmanagedType.Interface)] nsIURI aManifestURI, [MarshalAs(UnmanagedType.Interface)] nsIURI aDocumentURI, [MarshalAs(UnmanagedType.Interface)] nsIDOMDocument aDocument, [MarshalAs(UnmanagedType.Interface)] nsIFile aCustomProfileDir, uint aAppId, [MarshalAs(UnmanagedType.U1)] bool aInBrowser);
		
		/// <summary>
        /// Initialize the update for partial processing.
        ///
        /// @param aManifestURI
        /// The manifest URI of the related cache.
        /// @param aClientID
        /// Client  ID of the cache to store resource to. This ClientID
        /// must be ID of cache in the cache group identified by
        /// the manifest URI passed in the first parameter.
        /// @param aDocumentURI
        /// The page that is requesting the update. May be null
        /// when this information is unknown.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void InitPartial([MarshalAs(UnmanagedType.Interface)] nsIURI aManifestURI, [MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aClientID, [MarshalAs(UnmanagedType.Interface)] nsIURI aDocumentURI);
		
		/// <summary>
        /// Initialize the update to only check whether there is an update
        /// to the manifest available (if it has actually changed on the server).
        ///
        /// @param aManifestURI
        /// The manifest URI of the related cache.
        /// @param aAppID
        /// Local ID of an app (optional) to check the cache update for.
        /// @param aInBrowser
        /// Whether to check for a cache populated from browser element.
        /// @param aObserver
        /// nsIObserver implementation that receives the result.
        /// When aTopic == "offline-cache-update-available" there is an update to
        /// to download. Update of the app cache will lead to a new version
        /// download.
        /// When aTopic == "offline-cache-update-unavailable" then there is no
        /// update available (the manifest has not changed on the server).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void InitForUpdateCheck([MarshalAs(UnmanagedType.Interface)] nsIURI aManifestURI, uint aAppID, [MarshalAs(UnmanagedType.U1)] bool aInBrowser, [MarshalAs(UnmanagedType.Interface)] nsIObserver aObserver);
		
		/// <summary>
        /// Add a dynamic URI to the offline cache as part of the update.
        ///
        /// @param aURI
        /// The URI to add.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void AddDynamicURI([MarshalAs(UnmanagedType.Interface)] nsIURI aURI);
		
		/// <summary>
        /// Add the update to the offline update queue.  An offline-cache-update-added
        /// event will be sent to the observer service.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Schedule();
		
		/// <summary>
        /// Observe loads that are added to the update.
        ///
        /// @param aObserver
        /// object that notifications will be sent to.
        /// @param aHoldWeak
        /// TRUE if you want the update to hold a weak reference to the
        /// observer, FALSE for a strong reference.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void AddObserver([MarshalAs(UnmanagedType.Interface)] nsIOfflineCacheUpdateObserver aObserver, [MarshalAs(UnmanagedType.U1)] bool aHoldWeak);
		
		/// <summary>
        /// Remove an observer from the update.
        ///
        /// @param aObserver
        /// the observer to remove.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RemoveObserver([MarshalAs(UnmanagedType.Interface)] nsIOfflineCacheUpdateObserver aObserver);
		
		/// <summary>
        /// Cancel the update when still in progress. This stops all running resource
        /// downloads and discards the downloaded cache version. Throws when update
        /// has already finished and made the new cache version active.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Cancel();
		
		/// <summary>
        /// Return the number of bytes downloaded so far
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		ulong GetByteProgressAttribute();
	}
	
	/// <summary>nsIOfflineCacheUpdateService </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6ee353ba-11ea-4008-a78a-55b343fb2a49")]
	public interface nsIOfflineCacheUpdateService
	{
		
		/// <summary>
        /// Access to the list of cache updates that have been scheduled.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetNumUpdatesAttribute();
		
		/// <summary>Member GetUpdate </summary>
		/// <param name='index'> </param>
		/// <returns>A nsIOfflineCacheUpdate</returns>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIOfflineCacheUpdate GetUpdate(uint index);
		
		/// <summary>
        /// Schedule a cache update for a given offline manifest.  If an
        /// existing update is scheduled or running, that update will be returned.
        /// Otherwise a new update will be scheduled.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIOfflineCacheUpdate ScheduleUpdate([MarshalAs(UnmanagedType.Interface)] nsIURI aManifestURI, [MarshalAs(UnmanagedType.Interface)] nsIURI aDocumentURI, [MarshalAs(UnmanagedType.Interface)] nsIDOMWindow aWindow);
		
		/// <summary>
        /// Schedule a cache update for a given offline manifest and let the data
        /// be stored to a custom profile directory.  There is no coalescing of
        /// manifests by manifest URL.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIOfflineCacheUpdate ScheduleCustomProfileUpdate([MarshalAs(UnmanagedType.Interface)] nsIURI aManifestURI, [MarshalAs(UnmanagedType.Interface)] nsIURI aDocumentURI, [MarshalAs(UnmanagedType.Interface)] nsIFile aProfileDir);
		
		/// <summary>
        /// Schedule a cache update for a given offline manifest using app cache
        /// bound to the given appID+inBrowser flag.  If an existing update is
        /// scheduled or running, that update will be returned. Otherwise a new
        /// update will be scheduled.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIOfflineCacheUpdate ScheduleAppUpdate([MarshalAs(UnmanagedType.Interface)] nsIURI aManifestURI, [MarshalAs(UnmanagedType.Interface)] nsIURI aDocumentURI, uint aAppID, [MarshalAs(UnmanagedType.U1)] bool aInBrowser);
		
		/// <summary>
        /// Schedule a cache update for a manifest when the document finishes
        /// loading.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ScheduleOnDocumentStop([MarshalAs(UnmanagedType.Interface)] nsIURI aManifestURI, [MarshalAs(UnmanagedType.Interface)] nsIURI aDocumentURI, [MarshalAs(UnmanagedType.Interface)] nsIDOMDocument aDocument);
		
		/// <summary>
        /// Schedule a check to see if an update is available.
        ///
        /// This will not update or make any changes to the appcache.
        /// It only notifies the observer to indicate whether the manifest has
        /// changed on the server (or not): a changed manifest means that an
        /// update is available.
        ///
        /// For arguments see nsIOfflineCacheUpdate.initForUpdateCheck() method
        /// description.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CheckForUpdate([MarshalAs(UnmanagedType.Interface)] nsIURI aManifestURI, uint aAppID, [MarshalAs(UnmanagedType.U1)] bool aInBrowser, [MarshalAs(UnmanagedType.Interface)] nsIObserver aObserver);
		
		/// <summary>
        /// Checks whether a principal should have access to the offline
        /// cache.
        /// @param aPrincipal
        /// The principal to check.
        /// @param aPrefBranch
        /// The pref branch to use to check the
        /// offline-apps.allow_by_default pref.  If not specified,
        /// the pref service will be used.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool OfflineAppAllowed([MarshalAs(UnmanagedType.Interface)] nsIPrincipal aPrincipal, [MarshalAs(UnmanagedType.Interface)] nsIPrefBranch aPrefBranch);
		
		/// <summary>
        /// Checks whether a document at the given URI should have access
        /// to the offline cache.
        /// @param aURI
        /// The URI to check
        /// @param aPrefBranch
        /// The pref branch to use to check the
        /// offline-apps.allow_by_default pref.  If not specified,
        /// the pref service will be used.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool OfflineAppAllowedForURI([MarshalAs(UnmanagedType.Interface)] nsIURI aURI, [MarshalAs(UnmanagedType.Interface)] nsIPrefBranch aPrefBranch);
		
		/// <summary>
        /// Sets the "offline-app" permission for the principal.
        /// In the single process model calls directly on permission manager.
        /// In the multi process model dispatches to the parent process.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void AllowOfflineApp([MarshalAs(UnmanagedType.Interface)] nsIDOMWindow aWindow, [MarshalAs(UnmanagedType.Interface)] nsIPrincipal aPrincipal);
	}
	
	/// <summary>nsIOfflineCacheUpdateServiceConsts </summary>
	public class nsIOfflineCacheUpdateServiceConsts
	{
		
		// <summary>
        // Allow the domain to use offline APIs, and don't warn about excessive
        // usage.
        // </summary>
		public const ulong ALLOW_NO_WARN = 3;
	}
}
