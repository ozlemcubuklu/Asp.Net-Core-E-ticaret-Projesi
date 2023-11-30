using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Abstract
{
    public interface IValidation<T>
    {
        string ErrorMessage { get; set; }
        bool Validation(T entity);

    }
}
