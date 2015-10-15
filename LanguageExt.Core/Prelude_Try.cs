﻿using System;
using System.Linq.Expressions;
using System.Collections.Concurrent;
using System.Linq;
using LanguageExt.Trans;

namespace LanguageExt
{
    public static partial class Prelude
    {
        public static bool isSucc<T>(Try<T> value) =>
            !isFail(value);

        public static bool isFail<T>(Try<T> value) =>
            value.Try().IsFaulted;

        public static Unit ifSucc<T>(Try<T> tryDel, Action<T> Succ) =>
            tryDel.IfSucc(Succ);

        public static T ifFail<T>(Try<T> tryDel, Func<T> Fail) =>
            tryDel.IfFail(Fail);

        public static T ifFail<T>(Try<T> tryDel, T failValue) =>
            tryDel.IfFail(failValue);

        public static ExceptionMatch<T> ifFail<T>(Try<T> tryDel) =>
            tryDel.IfFail();

        public static Try<Exception> failed<T>(Try<T> tryDel) =>
            map(tryDel, 
                Succ: _  => new NotSupportedException(),
                Fail: ex => ex
                );

        public static Try<T> flatten<T>(Try<Try<T>> tryDel) =>
            tryDel.Flatten();

        public static Try<T> flatten<T>(Try<Try<Try<T>>> tryDel) =>
            tryDel.Flatten();

        public static Try<T> flatten<T>(Try<Try<Try<Try<T>>>> tryDel) =>
            tryDel.Flatten();

        public static R match<T, R>(Try<T> tryDel, Func<T, R> Succ, Func<Exception, R> Fail) =>
            tryDel.Match(Succ, Fail);

        public static R match<T, R>(Try<T> tryDel, Func<T, R> Succ, R Fail) =>
            tryDel.Match(Succ, Fail);

        public static Unit match<T>(Try<T> tryDel, Action<T> Succ, Action<Exception> Fail) =>
            tryDel.Match(Succ, Fail);

        /// <summary>
        /// Apply a Try value to a Try function
        /// </summary>
        /// <param name="self">Try function</param>
        /// <param name="arg">Try argument</param>
        /// <returns>Returns the result of applying the Try argument to the Try function</returns>
        public static Try<R> apply<T, R>(Try<Func<T, R>> self, Try<T> arg) =>
            self.Apply(arg);

        /// <summary>
        /// Apply a Try value to a Try function of arity 2
        /// </summary>
        /// <param name="self">Try function</param>
        /// <param name="arg">Try argument</param>
        /// <returns>Returns the result of applying the Try argument to the Try function:
        /// a Try function of arity 1</returns>
        public static Try<Func<T2, R>> apply<T1, T2, R>(Try<Func<T1, T2, R>> self, Try<T1> arg) =>
            self.Apply(arg);

        /// <summary>
        /// Apply Try values to a Try function of arity 2
        /// </summary>
        /// <param name="self">Try function</param>
        /// <param name="arg1">Try argument</param>
        /// <param name="arg2">Try argument</param>
        /// <returns>Returns the result of applying the Try arguments to the Try function</returns>
        public static Try<R> apply<T1, T2, R>(Try<Func<T1, T2, R>> self, Try<T1> arg1, Try<T2> arg2) =>
            self.Apply(arg1, arg2);

        public static Unit iter<T>(Try<T> self, Action<T> action) =>
            self.Iter(action);

        public static Unit iter<T>(Try<T> self, Action<T> Succ, Action<Exception> Fail) =>
            self.Iter(Succ, Fail);

        public static S fold<S, T>(Try<T> tryDel, S state, Func<S, T, S> folder) =>
            tryDel.Fold(state, folder);

        public static S fold<S, T>(Try<T> self, S state, Func<S, T, S> Succ, Func<S, Exception, S> Fail) =>
            self.Fold(state, Succ, Fail);

        public static bool forall<T>(Try<T> tryDel, Func<T, bool> pred) =>
            tryDel.ForAll(pred);

        public static bool forall<T>(Try<T> tryDel, Func<T, bool> Succ, Func<Exception, bool> Fail) =>
            tryDel.ForAll(Succ,Fail);

        public static int count<T>(Try<T> tryDel) =>
            tryDel.Count();

        public static bool exists<T>(Try<T> tryDel, Func<T, bool> pred) =>
            tryDel.Exists(pred);

        public static bool exists<T>(Try<T> self, Func<T, bool> Succ, Func<Exception, bool> Fail) =>
            self.Exists(Succ,Fail);

        public static Try<R> map<T, R>(Try<T> tryDel, Func<T, R> mapper) =>
            tryDel.Map(mapper);

        public static Try<R> map<T, R>(Try<T> tryDel, Func<T, R> Succ, Func<Exception, R> Fail) =>
            tryDel.Map(Succ, Fail);

        /// <summary>
        /// Partial application map
        /// </summary>
        /// <remarks>TODO: Better documentation of this function</remarks>
        public static Try<Func<T2, R>> map<T1, T2, R>(Try<T1> self, Func<T1, T2, R> func) =>
            self.Map(func);

        /// <summary>
        /// Partial application map
        /// </summary>
        /// <remarks>TODO: Better documentation of this function</remarks>
        public static Try<Func<T2, Func<T3, R>>> map<T1, T2, T3, R>(Try<T1> self, Func<T1, T2, T3, R> func) =>
            self.Map(func);

        public static Try<T> filter<T>(Try<T> self, Func<T, bool> pred) =>
            self.Filter(pred);

        public static Try<T> filter<T>(Try<T> self, Func<T, bool> Succ, Func<Exception, bool> Fail) =>
            self.Filter(Succ, Fail);

        public static Try<R> bind<T, R>(Try<T> tryDel, Func<T, Try<R>> binder) =>
            tryDel.Bind(binder);

        public static Try<R> bind<T, R>(Try<T> self, Func<T, Try<R>> Succ, Func<Exception, Try<R>> Fail) =>
            self.Bind(Succ, Fail);

        public static Lst<Either<Exception, T>> toList<T>(Try<T> tryDel) =>
            tryDel.ToList();

        public static Either<Exception, T>[] toArray<T>(Try<T> tryDel) =>
            tryDel.ToArray();

        public static IQueryable<Either<Exception, T>> toQuery<T>(Try<T> tryDel) =>
            tryDel.ToList().AsQueryable();

        public static Try<T> tryfun<T>(Func<Try<T>> tryDel) => () =>
        {
            try
            {
                return tryDel().Try();
            }
            catch (Exception e)
            {
                return new TryResult<T>(e);
            }
        };

        public static Try<T> Try<T>(Func<T> tryDel) => () =>
            tryDel();

    }
}
