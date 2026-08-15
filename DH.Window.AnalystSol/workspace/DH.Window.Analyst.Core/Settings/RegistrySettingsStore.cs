//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DH.Window.Analyst
//	Author			: CyberKDH
//	Module			: DH.Window.Analyst
//	History			:
//	Copyrights		: Copyright ⓒCYBERKDH. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.Win32;

namespace DH.Window.Analyst.Settings {
	// swallows registry access failures and falls back to the default instead of throwing, so a corrupt/missing value never crashes the app
	public class RegistrySettingsStore {
		private readonly string m_strSubKeyPath;

		public RegistrySettingsStore(string strsubkeypath) {
			m_strSubKeyPath = strsubkeypath;
		}

		public int GetInt(string strvaluename, int ndefault) {
			try {
				using (RegistryKey key = Registry.CurrentUser.CreateSubKey(m_strSubKeyPath)) {
					return Convert.ToInt32(key.GetValue(strvaluename, ndefault));
				}
			}
			catch (Exception) {
				return ndefault;
			}
		}

		public void SetInt(string strvaluename, int nvalue) {
			try {
				using (RegistryKey key = Registry.CurrentUser.CreateSubKey(m_strSubKeyPath)) {
					key.SetValue(strvaluename, nvalue, RegistryValueKind.DWord);
				}
			}
			catch (Exception) {
			}
		}

		public bool GetBool(string strvaluename, bool bdefault) {
			return GetInt(strvaluename, bdefault == true ? 1 : 0) != 0;
		}

		public void SetBool(string strvaluename, bool bvalue) {
			SetInt(strvaluename, bvalue == true ? 1 : 0);
		}

		public string GetString(string strvaluename, string strdefault) {
			try {
				using (RegistryKey key = Registry.CurrentUser.CreateSubKey(m_strSubKeyPath)) {
					return Convert.ToString(key.GetValue(strvaluename, strdefault));
				}
			}
			catch (Exception) {
				return strdefault;
			}
		}

		public void SetString(string strvaluename, string strvalue) {
			try {
				using (RegistryKey key = Registry.CurrentUser.CreateSubKey(m_strSubKeyPath)) {
					key.SetValue(strvaluename, strvalue ?? string.Empty, RegistryValueKind.String);
				}
			}
			catch (Exception) {
			}
		}
	}
}
