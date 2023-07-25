using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Controllers;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PokemonReviewApp.Tests.Controller
{
    public class PokemonControllerTests
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        public PokemonControllerTests()
        {
            _pokemonRepository = A.Fake<IPokemonRepository>();
            _reviewRepository = A.Fake<IReviewRepository>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public void PokemonController_GetPokemons_ReturnOK()
        {
            //Arrange
            var pokemons = A.Fake<ICollection<PokemonDto>>();
            var pokemonList = A.Fake<List<PokemonDto>>();
            A.CallTo(() => _mapper.Map<List<PokemonDto>>(pokemons)).Returns(pokemonList);
            var controller = new PokemonController(_pokemonRepository, _reviewRepository, _mapper);

            //Act
            var result = controller.GetPokemons();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void PokemonController_CreatePokemon_ReturnOK()
        {
            //Arrange
            int ownerId = 1;
            int catId = 2;
            var pokemonMap = A.Fake<Pokemon>();
            var pokemon = A.Fake<Pokemon>();
            var pokemonCreate = A.Fake<PokemonDto>();
            //var pokemons = A.Fake<ICollection<PokemonDto>>();
            //var pokmonList = A.Fake<IList<PokemonDto>>();
            A.CallTo(() => _pokemonRepository.GetPokemonTrimToUpper(pokemonCreate)).Returns(pokemon);
            A.CallTo(() => _mapper.Map<Pokemon>(pokemonCreate)).Returns(pokemon);
            A.CallTo(() => _pokemonRepository.CreatePokemon(ownerId, catId, pokemonMap)).Returns(true);
            var controller = new PokemonController(_pokemonRepository, _reviewRepository, _mapper);

            //Act
            var result = controller.CreatePokemon(ownerId, catId, pokemonCreate);

            //Assert
            result.Should().NotBeNull();
        }
        [Fact]
        public void PokemonController_UpdatePokemon_ReturnOK() 
        {
            //Arrange
            int ownerId = 1;
            int catId = 2;
            int pokeId= 3;
            var updatedPokemonDto = new PokemonDto
            {
                Id = pokeId,
                Name="Sunil"
                // Set other properties of the updated PokemonDto here
            };
            var updatedPokemon = new Pokemon
            {
                Id = pokeId,
                Name= "Sunil"
                // Set other properties of the updated Pokemon here
            };

            A.CallTo(() => _pokemonRepository.PokemonExists(pokeId)).Returns(true);
            A.CallTo(() => _mapper.Map<Pokemon>(updatedPokemonDto)).Returns(updatedPokemon);
            A.CallTo(() => _pokemonRepository.UpdatePokemon(ownerId, catId, updatedPokemon)).Returns(true);
            var controller = new PokemonController(_pokemonRepository, _reviewRepository, _mapper);

            //Act
            var result = controller.UpdatePokemon(pokeId,ownerId, catId, updatedPokemonDto);

            //Assert
            //result.Should().NotBeNull();
            //result.Should().BeOfType<OkObjectResult>();
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be("Successfully updated");
        }

        [Fact]
        public void PokemonController_DeletePokemon_ReturnOK()
        {
            //Arrange
            int pokeId = 3;
            var pokemon = A.Fake<Pokemon>();
            var pokemonDTO = A.Fake<PokemonDto>();
            A.CallTo(() => _mapper.Map<Pokemon>(pokemonDTO)).Returns(pokemon);
            A.CallTo(() => _pokemonRepository.PokemonExists(pokeId)).Returns(true);
            A.CallTo(() => _pokemonRepository.DeletePokemon(pokemon)).Returns(true);
            var controller = new PokemonController(_pokemonRepository, _reviewRepository, _mapper);

            // Act
            var result = controller.DeletePokemon(pokeId);

            // Assert
            result.Should().NotBeNull();
            //result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
            //var okResult = result as OkObjectResult;
            //okResult.StatusCode.Should().Be(200);
            //okResult.Value.Should().Be("Successfully deleted");
        }
    }
}
