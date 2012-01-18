﻿#region License
//
// Copyright (c) 2008-2012, DoLittle Studios and Komplett ASA
//
// Licensed under the Microsoft Permissive License (Ms-PL), Version 1.1 (the "License")
// With one exception :
//   Commercial libraries that is based partly or fully on Bifrost and is sold commercially, 
//   must obtain a commercial license.
//
// You may not use this file except in compliance with the License.
// You may obtain a copy of the license at 
//
//   http://bifrost.codeplex.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using Bifrost.Execution;
using Ninject.Syntax;

namespace Bifrost.Ninject
{
	/// <summary>
	/// Provides functionality for setting scope to a Ninject <see cref="IBindingInSyntax{T}">Binding syntax</see> 
	/// </summary>
	public static class BindingLifecycleExtensions
	{

		/// <summary>
		/// Set scope based on <see cref="BindingLifecycle">ActivationScope</see>
		/// </summary>
		/// <param name="syntax"><see cref="IBindingInSyntax{T}">Binding syntax</see> to set the scope for</param>
		/// <param name="lifecycle"><see cref="BindingLifecycle"/> to use</param>
        public static void WithLifecycle<T>(this IBindingInSyntax<T> syntax, BindingLifecycle lifecycle)
		{
			switch (lifecycle)
			{
				case BindingLifecycle.Singleton:
					syntax.InSingletonScope();
					break;

#if(!SILVERLIGHT)
				case BindingLifecycle.Request:
					syntax.InRequestScope();
					break;
#endif
				case BindingLifecycle.Thread:
					syntax.InThreadScope();
					break;

				case BindingLifecycle.Transient:
					syntax.InTransientScope();
					break;
			}
		}
	}
}