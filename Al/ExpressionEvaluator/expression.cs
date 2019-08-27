using System;
using System.Collections.Generic;
using System.Text;
using Al.Expressions;

   public struct Mathexpr
    {
       ExpressionEval eval;
       public Mathexpr(string code)
       {
           eval = new ExpressionEval(code);
       }


       public override string ToString()
       {
           return eval.Expression;
       }

       public void Define(string name, object val)
       {
           eval.SetVariable(name, val);
       }
       public static Mathexpr operator !(Mathexpr a)
       {
           a.eval._variables.Clear();
           return a;
       }

       public static Mathexpr operator +(Mathexpr a, AdditionalFunctionEventHandler ev)
       {
           a.eval.AdditionalFunctionEventHandler += ev;
           return a;
       }
       public static Mathexpr operator -(Mathexpr a, AdditionalFunctionEventHandler ev)
       {
           a.eval.AdditionalFunctionEventHandler -= ev;
           return a;
       }

       public static implicit operator double(Mathexpr a)
       {
           return a.eval.EvaluateDouble();
       }
       public static implicit operator long(Mathexpr a)
       {
           return a.eval.EvaluateLong();
       }
       public static implicit operator int(Mathexpr a)
       {
           return a.eval.EvaluateInt();
       }
       public static implicit operator bool(Mathexpr a)
       {
           return a.eval.EvaluateBool();
       }
       public static implicit operator Mathexpr(string a)
       {
           return new Mathexpr(a);
       }


    }

