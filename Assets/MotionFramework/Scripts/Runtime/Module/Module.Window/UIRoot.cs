﻿//--------------------------------------------------
// Motion Framework
// Copyright©2018-2020 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MotionFramework.Resource;
using MotionFramework.Event;

namespace MotionFramework.Window
{
	public abstract class UIRoot : IEnumerator
	{
		private AssetReference _assetRef;
		private AssetOperationHandle _handle;

		/// <summary>
		/// 实例化对象
		/// </summary>
		public GameObject Go { private set; get; }

		/// <summary>
		/// 是否加载完毕
		/// </summary>
		public bool IsDone { get { return _handle.IsDone; } }

		/// <summary>
		/// 是否准备完毕
		/// </summary>
		public bool IsPrepare { get { return Go != null; } }

		/// <summary>
		/// UI桌面
		/// </summary>
		public abstract GameObject UIDesktop { protected set; get; }

		/// <summary>
		/// UI相机
		/// </summary>
		public abstract Camera UICamera { protected set; get; }


		internal void InternalLoad(string location)
		{
			if (_assetRef == null)
			{
				_assetRef = new AssetReference(location);
				_handle = _assetRef.LoadAssetAsync<GameObject>();
				_handle.Completed += Handle_Completed;
			}
		}
		internal void InternalDestroy()
		{
			if (Go != null)
			{
				GameObject.Destroy(Go);
				Go = null;
			}

			if (_assetRef != null)
			{
				_assetRef.Release();
				_assetRef = null;
			}
		}
		private void Handle_Completed(AssetOperationHandle obj)
		{
			if (_handle.AssetObject == null)
				return;

			// 实例化对象
			Go = _handle.InstantiateObject;
			GameObject.DontDestroyOnLoad(Go);

			// 调用重载函数
			OnAssetLoad(Go);
		}
		protected abstract void OnAssetLoad(GameObject go);

		#region 异步相关
		bool IEnumerator.MoveNext()
		{
			return !IsDone;
		}
		void IEnumerator.Reset()
		{
		}
		object IEnumerator.Current
		{
			get { return null; }
		}
		#endregion
	}
}