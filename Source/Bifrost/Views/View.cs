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
using System;
using System.Linq;
using Bifrost.Entities;
using Bifrost.Extensions;

namespace Bifrost.Views
{
	/// <summary>
	/// Represents a <see cref="IView{T}"/>
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class View<T> : IView<T>
	{
		private readonly IEntityContext<T> _entityContext;

		/// <summary>
		/// Initializes a new instance of <see cref="View{T}">QueryRepository</see>
		/// </summary>
		/// <param name="entityContext">An <see cref="IEntityContext{T}">IEntityContext</see> to use for querying</param>
		public View(IEntityContext<T> entityContext)
		{
			_entityContext = entityContext;
		}

#pragma warning disable 1591 // Xml Comments
		public IQueryable<T> Query { get { return _entityContext.Entities; } }
        public T Get(Guid id)
        {
            if( typeof(T).HasInterface<IHaveId>() )
            {
                var queryable = (IQueryable<IHaveId>)Query;
                var instance = (from o in queryable
                                where o.Id == id
                                select o).SingleOrDefault();
                return (T)instance;
            }

            throw new ObjectDoesNotHaveIdException();
        }
#pragma warning restore 1591 // Xml Comments
	}
}