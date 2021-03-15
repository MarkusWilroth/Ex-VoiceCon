//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.8
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace Pocketsphinx {

public class NGramModelSet : global::System.Collections.IEnumerable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal NGramModelSet(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(NGramModelSet obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~NGramModelSet() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          SphinxBasePINVOKE.delete_NGramModelSet(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator() {
     return (global::System.Collections.IEnumerator) GetEnumerator();
  }

  public NGramModelSetIterator GetEnumerator() {
    global::System.IntPtr cPtr = SphinxBasePINVOKE.NGramModelSet_GetEnumerator(swigCPtr);
    NGramModelSetIterator ret = (cPtr == global::System.IntPtr.Zero) ? null : new NGramModelSetIterator(cPtr, true);
    return ret;
  }

  public NGramModelSet(Config config, LogMath logmath, string path) : this(SphinxBasePINVOKE.new_NGramModelSet(Config.getCPtr(config), LogMath.getCPtr(logmath), path), true) {
  }

  public int Count() {
    int ret = SphinxBasePINVOKE.NGramModelSet_Count(swigCPtr);
    return ret;
  }

  public NGramModel Add(NGramModel model, string name, float weight, bool reuse_widmap) {
    global::System.IntPtr cPtr = SphinxBasePINVOKE.NGramModelSet_Add(swigCPtr, NGramModel.getCPtr(model), name, weight, reuse_widmap);
    NGramModel ret = (cPtr == global::System.IntPtr.Zero) ? null : new NGramModel(cPtr, false);
    return ret;
  }

  public NGramModel Select(string name) {
    global::System.IntPtr cPtr = SphinxBasePINVOKE.NGramModelSet_Select(swigCPtr, name);
    NGramModel ret = (cPtr == global::System.IntPtr.Zero) ? null : new NGramModel(cPtr, false);
    return ret;
  }

  public NGramModel Lookup(string name) {
    global::System.IntPtr cPtr = SphinxBasePINVOKE.NGramModelSet_Lookup(swigCPtr, name);
    NGramModel ret = (cPtr == global::System.IntPtr.Zero) ? null : new NGramModel(cPtr, false);
    return ret;
  }

  public string Current() {
    string ret = SphinxBasePINVOKE.NGramModelSet_Current(swigCPtr);
    return ret;
  }

}

}
