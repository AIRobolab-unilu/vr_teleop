using System.Collections;
using System.Text;
using SimpleJSON;
using UnityEngine;

/* 
 * @brief ROSBridgeLib
 * @author Michael Jenkin, Robert Codd-Downey, Andrew Speers and Miquel Massot Campos
 */

namespace ROSBridgeLib {
	namespace std_msgs {
		public class StringMsg : ROSBridgeMsg {
			private string _data;
			
			public StringMsg(JSONNode msg) {
                Debug.Log("okok");
				_data = msg["data"];
			}
			
			public StringMsg(string data) {
                Debug.Log("okok");
                _data = data;
			}
			
			public static string GetMessageType() {
				return "std_msgs/String";
			}
			
			public string GetData() {
				return _data;
			}
			
			public override string ToString() {
				return "String [data=" + _data + "]";
			}
			
			public override string ToYAMLString() {
                
                return "{\"data\" : \"" + _data + "\"}";
			}
		}
	}
}