﻿using Ardalis.Specification.EntityFrameworkCore;
using Ardalis.Specification;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RepositoryTutorial.Common;
using RepositoryTutorial.Domain;

namespace RepositoryTutorial.Infrastructure
{
    public class RepositoryAsync : IRepositoryAsync
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RepositoryAsync(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region [-- GET --]

        // get all, return non-paginated list of domain entities
        public async Task<IEnumerable<T>> GetListAsync<T, TId>(ISpecification<T> specification = null, CancellationToken cancellationToken = default) where T : BaseEntity<TId>
        {

            IQueryable<T> query;
            if (specification == null)
            {
                query = _context.Set<T>().AsQueryable();
            }
            else
            {
                query = SpecificationEvaluator.Default.GetQuery(
                  query: _context.Set<T>().AsQueryable(),
                  specification: specification);
            }
            List<T> result = await query.ToListAsync(cancellationToken);
            return result;
        }

        // get all, return non-paginated list of mapped dtos
        public async Task<IEnumerable<TDto>> GetListAsync<T, TDto, TId>(ISpecification<T> specification = null, CancellationToken cancellationToken = default) where T : BaseEntity<TId>
            where TDto : IDto
        {

            IQueryable<T> query;
            if (specification == null)
            {
                query = _context.Set<T>().AsQueryable();
            }
            else
            {
                query = SpecificationEvaluator.Default.GetQuery(
                  query: _context.Set<T>().AsQueryable(),
                  specification: specification);
            }

            List<TDto> result = await query
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return result;
        }

        // get by Id, return domain entity
        public async Task<T> GetByIdAsync<T, TId>(TId id, ISpecification<T> specification = null, CancellationToken cancellationToken = default) where T : BaseEntity<TId>
        {
            IQueryable<T> query;
            if (specification == null)
            {
                query = _context.Set<T>().AsQueryable();
            }
            else
            {
                query = SpecificationEvaluator.Default.GetQuery(
                  query: _context.Set<T>().AsQueryable(),
                  specification: specification);
            }

            T entity = await query.Where(x => x.Id.Equals(id)).FirstOrDefaultAsync(cancellationToken);

            if (entity != null)
            {
                return entity;
            }
            else
            {
                throw new Exception("Not Found");
            }

        }

        // get by Id, return mapped dtos
        public async Task<TDto> GetByIdAsync<T, TDto, TId>(TId id, ISpecification<T> specification = null, CancellationToken cancellationToken = default)
            where T : BaseEntity<TId>
            where TDto : IDto
        {
            IQueryable<T> query;
            if (specification == null)
            {
                query = _context.Set<T>().AsQueryable();
            }
            else
            {
                query = SpecificationEvaluator.Default.GetQuery(
                  query: _context.Set<T>().AsQueryable(),
                  specification: specification);
            }

            TDto result = await query
                .Where(x => x.Id.Equals(id))
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new Exception("Not Found");

            return result;

        }

        // check if exists, return true/false
        public async Task<bool> ExistsAsync<T, TId>(ISpecification<T> specification = null, CancellationToken cancellationToken = default)
        where T : BaseEntity<TId>
        {
            IQueryable<T> query;
            if (specification == null)
            {
                query = _context.Set<T>().AsQueryable();
            }
            else
            {
                query = SpecificationEvaluator.Default.GetQuery(
                  query: _context.Set<T>().AsQueryable(),
                  specification: specification);
            }

            bool result = await query.AnyAsync(cancellationToken);
            return result;
        }

        #endregion [-- GET --]

        #region [-- CREATE --]

        // create
        public async Task<T> CreateAsync<T, TId>(T entity)
        where T : BaseEntity<TId>
        {
            _ = await _context.Set<T>().AddAsync(entity);
            return entity;

        }

        // create range, retun list of guid
        public async Task<IList<TId>> CreateRangeAsync<T, TId>(IEnumerable<T> entityList)
        where T : BaseEntity<TId>
        {
            await _context.Set<T>().AddRangeAsync(entityList);
            return entityList.Select(x => x.Id).ToList();
        }
        #endregion [-- CREATE --]

        #region [-- UPDATE --]

        // update
        public async Task<T> UpdateAsync<T, TId>(T entity)
            where T : BaseEntity<TId>
        {
            if (_context.Entry(entity).State == EntityState.Unchanged)
            {
                throw new Exception("Nothing to update");
            }

            T entityInDb = await _context.Set<T>().FindAsync(entity.Id);
            if (entityInDb != null)
            {
                _context.Entry(entityInDb).CurrentValues.SetValues(entity);
                return entity;
            }
            else
            {
                throw new Exception("Not Found");
            }

        }
        #endregion [-- UPDATE --]

        #region [-- REMOVE --]

        // remove by entity
        public Task RemoveAsync<T, TId>(T entity) where T : BaseEntity<TId>
        {
            _ = _context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        // remove by Id
        public async Task<T> RemoveByIdAsync<T, TId>(TId entityId)
        where T : BaseEntity<TId>
        {
            T entity = await _context.Set<T>().FindAsync(entityId);
            if (entity == null)
            {
                throw new Exception("Not Found");
            }

            _ = _context.Set<T>().Remove(entity);

            return entity;
        }
        #endregion [-- REMOVE --]

        #region [-- PAGINATION --]

        // return paginated list of mapped dtos -- format specific to Tanstack Table v8 (React, Vue)
        public async Task<PaginatedResponse<TDto>> GetTanstackPaginatedResultsAsync<T, TDto, TId>(int pageNumber, int pageSize, ISpecification<T> specification = null, CancellationToken cancellationToken = default)
            where T : BaseEntity<TId>
            where TDto : IDto
        {

            IQueryable<T> query;
            if (specification == null)
            {
                query = _context.Set<T>().AsQueryable();
            }
            else
            {
                query = SpecificationEvaluator.Default.GetQuery(
                  query: _context.Set<T>().AsQueryable(),
                  specification: specification);
            }

            List<TDto> pagedResult;
            int recordsTotal;
            try
            {
                recordsTotal = await query.CountAsync(cancellationToken);
                pagedResult = await query
                    .Skip((pageNumber - 1) * pageSize).Take(pageSize)
                    .ProjectTo<TDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return new PaginatedResponse<TDto>(pagedResult, recordsTotal, pageNumber, pageSize);
        }

      

        #endregion [-- PAGINATION --]

        #region [-- SAVE --]

        // save the changes to database
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #endregion [-- SAVE --]
    }
}
