
using OFrameLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace OFrameLibrary.Util
{
    /// <summary>
    /// Unit of Work class responsible for DB transactions
    /// </summary>
    public class UnitOfWork //: IDisposable
    {
        //#region Private member variables...

        //private GalleryEntities _context = null;
        //private GenericRepository<GY_AudioCategories> _audioCategoriesRepository;
        //#endregion

        //public UnitOfWork()
        //{
        //    _context = new GalleryEntities();
        //}

        //#region Public Repository Creation properties...

        ///// <summary>
        ///// Get/Set Property for product repository.
        ///// </summary>
        //public GenericRepository<GY_AudioCategories> AudioCategoriesRepository
        //{
        //    get
        //    {
        //        if (this._audioCategoriesRepository == null)
        //            this._audioCategoriesRepository = new GenericRepository<GY_AudioCategories>(_context);
        //        return _audioCategoriesRepository;
        //    }
        //}
        //#endregion

        //#region Public member methods...
        ///// <summary>
        ///// Save method.
        ///// </summary>
        //public void Save()
        //{
        //    try
        //    {
        //        _context.SaveChanges();
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        var msg = ExceptionHelper.GetEntityExceptionMessage(e);

        //        throw;
        //    }

        //}

        //#endregion

        //#region Implementing IDiosposable...

        //#region private dispose variable declaration...
        //private bool disposed = false;
        //#endregion

        ///// <summary>
        ///// Protected Virtual Dispose method
        ///// </summary>
        ///// <param name="disposing"></param>
        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!this.disposed)
        //    {
        //        if (disposing)
        //        {
        //            Debug.WriteLine("UnitOfWork is being disposed");
        //            _context.Dispose();
        //        }
        //    }
        //    this.disposed = true;
        //}

        ///// <summary>
        ///// Dispose method
        ///// </summary>
        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}
        //#endregion
    }
}