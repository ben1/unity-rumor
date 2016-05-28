﻿using Exodrifter.Rumor.Expressions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Exodrifter.Rumor.Engine
{
	/// <summary>
	/// A scope which keeps track of declared variables and functions.
	/// </summary>
	[Serializable]
	public class Scope : ISerializable
	{
		/// <summary>
		/// The variables in this scope.
		/// </summary>
		private readonly Dictionary<string, Value> vars;

		/// <summary>
		/// Creates a new scope.
		/// </summary>
		/// <param name="parent">
		/// The parent scope or null if there is none.
		/// </param>
		public Scope()
		{
			vars = new Dictionary<string, Value>();
		}

		/// <summary>
		/// Sets the value of one variable to an int.
		/// </summary>
		/// <param name="name">The name of the variable to set.</param>
		/// <param name="value">The value of the variable to use.</param>
		public void SetVar(string name, int @int)
		{
			vars[name] = new IntValue(@int);
		}

		/// <summary>
		/// Sets the value of one variable to a float.
		/// </summary>
		/// <param name="name">The name of the variable to set.</param>
		/// <param name="value">The value of the variable to use.</param>
		public void SetVar(string name, float @float)
		{
			vars[name] = new FloatValue(@float);
		}

		/// <summary>
		/// Sets the value of one variable to a string.
		/// </summary>
		/// <param name="name">The name of the variable to set.</param>
		/// <param name="value">The value of the variable to use.</param>
		public void SetVar(string name, string @string)
		{
			vars[name] = new StringValue(@string);
		}

		/// <summary>
		/// Sets the value of one variable to another.
		/// </summary>
		/// <param name="name">The name of the variable to set.</param>
		/// <param name="value">The value of the variable to use.</param>
		public void SetVar(string name, Value value)
		{
			vars[name] = value;
		}

		/// <summary>
		/// Returns the value of the specified variable.
		/// </summary>
		/// <returns>The value of the variable.</returns>
		/// <param name="name">The name of the variable to get.</param>
		public Value GetVar(string name)
		{
			if (vars.ContainsKey(name)) {
				return vars[name];
			}
			return null;
		}

		/// <summary>
		/// Returns true if this scope or a parent scope has the specified
		/// variable.
		/// </summary>
		/// <param name="name">The name of the variable to find.</param>
		/// <returns>True if the variable is declared.</returns>
		public bool HasVar(string name)
		{
			return vars.ContainsKey(name);
		}

		/// <summary>
		/// Removes all of the variables from this scope.
		/// </summary>
		/// <param name="recursive">If true, clear parent scopes too.</param>
		public void Clear(bool recursive)
		{
			vars.Clear();
		}

		#region Serialization

		public Scope(SerializationInfo info, StreamingContext context)
		{
			var keys = (List<string>)info.GetValue("keys", typeof(List<string>));
			var values = (List<Value>)info.GetValue("values", typeof(List<Value>));

			vars = new Dictionary<string, Value>();
			for (int i = 0; i < keys.Count; ++i) {
				vars.Add(keys[i], values[i]);
			}
		}

		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			var keys = new List<string>(vars.Count);
			var values = new List<Value>(vars.Count);

			foreach (var kvp in vars) {
				keys.Add(kvp.Key);
				values.Add(kvp.Value);
			}

			info.AddValue("keys", keys, typeof(List<string>));
			info.AddValue("values", values, typeof(List<Value>));
		}

		#endregion
	}
}