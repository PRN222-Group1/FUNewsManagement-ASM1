﻿using AutoMapper;
using BusinessServiceLayer.DTOs;
using BusinessServiceLayer.Interfaces;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Specifications.Tags;

namespace BusinessServiceLayer.Services
{
    public class TagService : ITagService
    {
        private readonly IGenericRepository<Tag> _repository;
        private readonly IMapper _mapper;

        public TagService(IGenericRepository<Tag> repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> CreateTagAsync(TagToAddOrUpdateDTO tag)
        {
            var result = false;
            var tagToAdd = _mapper.Map<TagToAddOrUpdateDTO, Tag>(tag);

            _repository.Add(tagToAdd);
            result = await _repository.SaveAllAsync();

            return result;
        }

        public async Task<TagDTO> GetTagByIdAsync(int id)
        {
            var tag = await _repository.GetByIdAsync(id);
            return _mapper.Map<Tag, TagDTO>(tag);
        }

        public async Task<IReadOnlyList<TagDTO>> GetTagsAsync(string? search)
        {
            var spec = new TagSpecification(search);
            var tags = await _repository.ListAsync(spec);
            return _mapper.Map<IReadOnlyList<Tag>, IReadOnlyList<TagDTO>>(tags);
        }

        public async Task<bool> UpdateTagAsync(int id, TagToAddOrUpdateDTO tag)
        {
            bool result = false;
            var tagToUpdate = await _repository.GetByIdAsync(id);

            if (tagToUpdate == null)
            {
                return result;
            }

            tagToUpdate.TagName = tag.TagName;
            tagToUpdate.Note = tag.Note;

            _repository.Update(tagToUpdate);
            result = await _repository.SaveAllAsync();

            return result;
        }
    }
}
