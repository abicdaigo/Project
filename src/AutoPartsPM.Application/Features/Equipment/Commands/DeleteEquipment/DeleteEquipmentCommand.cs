using AutoPartsPM.Application.Common.Models;
using MediatR;

namespace AutoPartsPM.Application.Features.Equipment.Commands.DeleteEquipment;

public record DeleteEquipmentCommand(int Id) : IRequest<Result>;
