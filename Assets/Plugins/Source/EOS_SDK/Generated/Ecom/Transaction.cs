// Copyright Epic Games, Inc. All Rights Reserved.
// This file is automatically generated. Changes to this file may be overwritten.

namespace Epic.OnlineServices.Ecom
{
	public sealed partial class Transaction : Handle
	{
		public Transaction()
		{
		}

		public Transaction(System.IntPtr innerHandle) : base(innerHandle)
		{
		}

		/// <summary>
		/// The most recent version of the <see cref="CopyEntitlementByIndex" /> Function.
		/// </summary>
		public const int TransactionCopyentitlementbyindexApiLatest = 1;

		/// <summary>
		/// The most recent version of the <see cref="GetEntitlementsCount" /> Function.
		/// </summary>
		public const int TransactionGetentitlementscountApiLatest = 1;

		/// <summary>
		/// Fetches an entitlement from a given index.
		/// <seealso cref="EcomInterface.Release" />
		/// </summary>
		/// <param name="options">structure containing the index being accessed</param>
		/// <param name="outEntitlement">the entitlement for the given index, if it exists and is valid, use <see cref="EcomInterface.Release" /> when finished</param>
		/// <returns>
		/// <see cref="Result.Success" /> if the information is available and passed out in OutEntitlement
		/// <see cref="Result.EcomEntitlementStale" /> if the entitlement information is stale and passed out in OutEntitlement
		/// <see cref="Result.InvalidParameters" /> if you pass a null pointer for the out parameter
		/// <see cref="Result.NotFound" /> if the entitlement is not found
		/// </returns>
		public Result CopyEntitlementByIndex(TransactionCopyEntitlementByIndexOptions options, out Entitlement outEntitlement)
		{
			var optionsAddress = System.IntPtr.Zero;
			Helper.TryMarshalSet<TransactionCopyEntitlementByIndexOptionsInternal, TransactionCopyEntitlementByIndexOptions>(ref optionsAddress, options);

			var outEntitlementAddress = System.IntPtr.Zero;

			var funcResult = Bindings.EOS_Ecom_Transaction_CopyEntitlementByIndex(InnerHandle, optionsAddress, ref outEntitlementAddress);

			Helper.TryMarshalDispose(ref optionsAddress);

			if (Helper.TryMarshalGet<EntitlementInternal, Entitlement>(outEntitlementAddress, out outEntitlement))
			{
				Bindings.EOS_Ecom_Entitlement_Release(outEntitlementAddress);
			}

			return funcResult;
		}

		/// <summary>
		/// Fetch the number of entitlements that are part of this transaction.
		/// <seealso cref="CopyEntitlementByIndex" />
		/// </summary>
		/// <param name="options">structure containing the Epic Online Services Account ID being accessed</param>
		/// <returns>
		/// the number of entitlements found.
		/// </returns>
		public uint GetEntitlementsCount(TransactionGetEntitlementsCountOptions options)
		{
			var optionsAddress = System.IntPtr.Zero;
			Helper.TryMarshalSet<TransactionGetEntitlementsCountOptionsInternal, TransactionGetEntitlementsCountOptions>(ref optionsAddress, options);

			var funcResult = Bindings.EOS_Ecom_Transaction_GetEntitlementsCount(InnerHandle, optionsAddress);

			Helper.TryMarshalDispose(ref optionsAddress);

			return funcResult;
		}

		/// <summary>
		/// The Ecom Transaction Interface exposes getters for accessing information about a completed transaction.
		/// All Ecom Transaction Interface calls take a handle of type <see cref="Transaction" /> as the first parameter.
		/// An <see cref="Transaction" /> handle is originally returned as part of the <see cref="CheckoutCallbackInfo" /> struct.
		/// An <see cref="Transaction" /> handle can also be retrieved from an <see cref="EcomInterface" /> handle using <see cref="EcomInterface.CopyTransactionByIndex" />.
		/// It is expected that after a transaction that <see cref="Release" /> is called.
		/// When <see cref="Platform.PlatformInterface.Release" /> is called any remaining transactions will also be released.
		/// <seealso cref="CheckoutCallbackInfo" />
		/// <seealso cref="EcomInterface.GetTransactionCount" />
		/// <seealso cref="EcomInterface.CopyTransactionByIndex" />
		/// </summary>
		public Result GetTransactionId(out string outBuffer)
		{
			System.IntPtr outBufferAddress = System.IntPtr.Zero;
			int inOutBufferLength = EcomInterface.TransactionidMaximumLength + 1;
			Helper.TryMarshalAllocate(ref outBufferAddress, inOutBufferLength);

			var funcResult = Bindings.EOS_Ecom_Transaction_GetTransactionId(InnerHandle, outBufferAddress, ref inOutBufferLength);

			Helper.TryMarshalGet(outBufferAddress, out outBuffer);
			Helper.TryMarshalDispose(ref outBufferAddress);

			return funcResult;
		}

		/// <summary>
		/// Release the memory associated with an <see cref="Transaction" />. Is is expected to be called after
		/// being received from a <see cref="CheckoutCallbackInfo" />.
		/// <seealso cref="CheckoutCallbackInfo" />
		/// <seealso cref="EcomInterface.GetTransactionCount" />
		/// <seealso cref="EcomInterface.CopyTransactionByIndex" />
		/// </summary>
		/// <param name="transaction">A handle to a transaction.</param>
		public void Release()
		{
			Bindings.EOS_Ecom_Transaction_Release(InnerHandle);
		}
	}
}