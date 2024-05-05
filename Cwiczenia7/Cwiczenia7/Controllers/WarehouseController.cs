using Cwiczenia7.Exceptions;
using Cwiczenia7.Models;
using Cwiczenia7.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cwiczenia7.Controllers;

[ApiController]
[Route("api/warehouses")]
public class WarehouseController(IWarehouseService service) : ControllerBase
{
    private readonly IWarehouseService _service = service;
    
    [HttpGet("zad1")]
    public async Task<IActionResult> FulfillOrderAsync(RequestDto requestDto)
    {
        try
        {
            var warehouseProductId = await _service.AddProductToWarehouseAsync(requestDto.ProductId,
                requestDto.WarehouseId, requestDto.Amount, requestDto.CreatedAt);

            return Ok(warehouseProductId);
        }
        catch (Exception e) when
            (e is NoOrderForProductException or NoSuchProductException or NoSuchWarehouseException)
        {
            return StatusCode(404, e.Message);
        }
        catch (Exception e) when
            (e is IllegalDateOfCreationException or IllegalProductAmountException or OrderAlreadyFulfilledException)
        {
            return StatusCode(400, e.Message);
        }
    }
    
}