using System;
using Microsoft.AspNetCore.Mvc;

namespace Skad.Common.Domain.Service
{
    public abstract class Result<TEntity>
    {
        public static Result<TEntity> Success(TEntity result)
        {
            return new SuccessResult<TEntity>(result);
        }
    }

    internal class SuccessResult<TEntity> : Result<TEntity>
    {
        private readonly TEntity _result;

        public SuccessResult(TEntity result)
        { 
            _result = result;
        }
        
        public ActionResult Transform(Func<TEntity, ActionResult> f)
        {
            return f(_result);
        }
    }

    internal class NotFoundResult<TEntity> : Result<TEntity>
    {
        private readonly string _errorMsg;

        public NotFoundResult(string errorMsg)
        {
            _errorMsg = errorMsg;
        }

        public TRep Transform<TRep>(Func<TRep> f)
        {
            return f();
        }
    }

    internal class AlreadyExistsResult<TEntity> : Result<TEntity>
    {
        public TRep Transform<TRep>(Func<TRep> f)
        {
            return f();
        }
    }
}