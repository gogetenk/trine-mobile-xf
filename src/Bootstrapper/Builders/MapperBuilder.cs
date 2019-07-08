using System.Collections.Generic;
using System.Linq;
using Assistance.Client.Dto;
using AutoMapper;
using Trine.Mobile.Dal.Swagger;
using Trine.Mobile.Dto;
using Trine.Mobile.Model;
using Xamarin.Forms;
using static Trine.Mobile.Model.MissionModel;

namespace Trine.Mobile.Bootstrapper.Builders
{
    internal class MapperBuilder
    {
        public MapperBuilder()
        {
        }

        public IMapper CreateMapper()
        {
            var config = new MapperConfiguration((cfg) =>
            {
                MapMissions(cfg);
                cfg.CreateMap<FrameContract, FrameContractModel>()
                    .ForPath(x => x.Id, opts => opts.Ignore())
                    .ForPath(x => x.FileUri, opts => opts.Ignore());
                cfg.CreateMap<Invoice, InvoiceModel>();
                cfg.CreateMap<InvoiceModel, InvoiceDto>()
                    .ForMember(x => x.PinColor, opts => opts.MapFrom(x => GetPinColorFromInvoiceStatus(x.State)));
                cfg.CreateMap<Event, EventModel>();
                cfg.CreateMap<RegisterUserDto, RegisterUserModel>();
                cfg.CreateMap<RegisterUserModel, RegisterUserRequest>();
                cfg.CreateMap<Activity, ActivityModel>();
                cfg.CreateMap<SubContract, SubContractModel>();
                cfg.CreateMap<DashboardMissionModel, DashboardMissionDto>();
                cfg.CreateMap<DashboardCountModel, DashboardCountDto>();
                cfg.CreateMap<DashboardCustomerModel, DashboardCustomerDto>();
                cfg.CreateMap<User, UserModel>();
                cfg.CreateMap<UserModel, UserDto>()
                    .ForMember(x => x.DisplayName, opts => opts.MapFrom(x => $"{x.Firstname} {x.Lastname.ToUpperInvariant()}"));

                cfg.CreateMap<ActivityModel, ActivityDto>()
                    .ForMember(x => x.PinColor, opts => opts.MapFrom(x => GetPinFromActivityStatus(x.Status)))
                    .ForMember(x => x.TranscodedStatus, opts => opts.MapFrom(x => GetStatusTranscription(x.Status)))
                    .ForMember(x => x.DaysNb, opts => opts.MapFrom(x => CalculateWorkedDays(x.Days)));

                cfg.CreateMap<OrganizationModel, Organization>();
                cfg.CreateMap<OrganizationMemberModel, OrganizationMember>();
                cfg.CreateMap<Organization, OrganizationModel>();
                cfg.CreateMap<OrganizationDto, OrganizationModel>();
                cfg.CreateMap<OrganizationModel, OrganizationDto>();
                cfg.CreateMap<PartialOrganizationModel, PartialOrganizationDto>()
                    .ForMember(x => x.Initials, opts => opts.MapFrom(x => $"{x.Name[0].ToString().ToUpper()}{x.Name[1].ToString().ToUpper()}"));

                cfg.CreateMap<Token, TokenModel>();

                cfg.CreateMissingTypeMaps = true;
                cfg.AllowNullCollections = true;
            });

            return config.CreateMapper();
        }

        private void MapMissions(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Mission, MissionModel>();
            cfg.CreateMap<MissionModel, MissionDto>()
                    .ForMember(x => x.PinColor, opts => opts.MapFrom(x => GetPinColorFromStatus(x.Status)))
                    .ForMember(x => x.StatusText, opts => opts.MapFrom(x => GetMissionStatusTranscription(x.Status)))
                    .ForMember(x => x.BadgeIconText, opts => opts.MapFrom(x => GetMissionStatusIcon(x.Status)));
            cfg.CreateMap<MissionDto, MissionModel>();
            cfg.CreateMap<MissionModel, Mission>();

        }


        private string GetStatusTranscription(Model.ActivityStatusEnum status)
        {
            string statusName;

            switch (status)
            {
                case Model.ActivityStatusEnum.Generated:
                    statusName = "A remplir";
                    break;
                case Model.ActivityStatusEnum.CustomerSigned:
                    statusName = "Signé par le client";
                    break;
                case Model.ActivityStatusEnum.ConsultantSigned:
                    statusName = "Signé par le consultant";
                    break;
                default:
                    statusName = "error";
                    break;
            }

            return statusName;
        }

        private string GetMissionStatusTranscription(StatusEnum status)
        {
            string statusName;

            switch (status)
            {
                case StatusEnum.CREATED:
                    statusName = "En attente de validation";
                    break;
                case StatusEnum.CONFIRMED:
                    statusName = "En cours";
                    break;
                case StatusEnum.CANCELED:
                    statusName = "Terminée";
                    break;
                default:
                    statusName = "error";
                    break;
            }

            return statusName;
        }

        private string GetMissionStatusIcon(StatusEnum status)
        {
            string statusName;

            switch (status)
            {
                case StatusEnum.CREATED:
                    statusName = "...";
                    break;
                case StatusEnum.CONFIRMED:
                    statusName = "";
                    break;
                case StatusEnum.CANCELED:
                    statusName = "";
                    break;
                default:
                    statusName = "error";
                    break;
            }

            return statusName;
        }

        private float CalculateWorkedDays(List<GridDayModel> days)
        {
            float fulldays = days
                .Count(x => (x.WorkedPart == Model.DayPartEnum.Full));

            float halfdays = days
                .Count(x => (x.WorkedPart == Model.DayPartEnum.Afternoon) || x.WorkedPart == Model.DayPartEnum.Morning);

            return fulldays + (halfdays / 2f);
        }


        private Color GetPinColorFromStatus(MissionModel.StatusEnum status)
        {
            string colorHex;

            switch (status)
            {
                case MissionModel.StatusEnum.CREATED:
                    colorHex = "#F0B429";
                    break;
                case MissionModel.StatusEnum.CONFIRMED:
                    colorHex = "#3EBD93";
                    break;
                case MissionModel.StatusEnum.CANCELED:
                    colorHex = "#E12D38";
                    break;
                default:
                    colorHex = "#F0B429";
                    break;
            }

            return Color.FromHex(colorHex);
        }

        private Color GetPinColorFromInvoiceStatus(InvoiceModel.StateEnum status)
        {
            string colorHex;

            switch (status)
            {
                case InvoiceModel.StateEnum.CREATED:
                    colorHex = "#F0B429";
                    break;
                case InvoiceModel.StateEnum.PAID:
                    colorHex = "#6CC5CC";
                    break;
                case InvoiceModel.StateEnum.CANCELED:
                    colorHex = "#ED8884";
                    break;
                case InvoiceModel.StateEnum.WAITING_PAYMENT:
                    colorHex = "#e67e22";
                    break;
                default:
                    colorHex = "#6CC5CC";
                    break;
            }

            return Color.FromHex(colorHex);
        }

        private Color GetPinColorFromEventType(EventModel.EventTypeEnum eventType)
        {
            string colorHex;

            switch (eventType)
            {
                case EventModel.EventTypeEnum.INFO:
                    colorHex = "#3498db";
                    break;
                case EventModel.EventTypeEnum.ACTION:
                    colorHex = "#1abc9c";
                    break;
                case EventModel.EventTypeEnum.ALERT:
                    colorHex = "#f1c40f";
                    break;
                case EventModel.EventTypeEnum.FATAL:
                    colorHex = "#e74c3c";
                    break;
                default:
                    colorHex = "#3498db";
                    break;
            }

            return Color.FromHex(colorHex);
        }

        private string GetIcontypeFromEventType(EventModel.EventTypeEnum eventType)
        {
            string icon;

            switch (eventType)
            {
                case EventModel.EventTypeEnum.INFO:
                    icon = "p";
                    break;
                case EventModel.EventTypeEnum.ACTION:
                    icon = "R";
                    break;
                case EventModel.EventTypeEnum.ALERT:
                    icon = "r";
                    break;
                case EventModel.EventTypeEnum.FATAL:
                    icon = "s";
                    break;
                default:
                    icon = "p";
                    break;
            }

            return icon;
        }

        private Color GetPinFromActivityStatus(Model.ActivityStatusEnum status)
        {
            string colorHex;

            switch (status)
            {
                case Model.ActivityStatusEnum.Generated:
                    colorHex = "#f1c40f";
                    break;
                case Model.ActivityStatusEnum.CustomerSigned:
                    colorHex = "#16a085";
                    break;
                case Model.ActivityStatusEnum.ConsultantSigned:
                    colorHex = "#2980b9";
                    break;
                default:
                    colorHex = "#16a085";
                    break;
            }

            return Color.FromHex(colorHex);
        }
    }
}
