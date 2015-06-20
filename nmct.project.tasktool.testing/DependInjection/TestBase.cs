using Microsoft.VisualStudio.TestTools.UnitTesting;
using nmct.project.tasktool.businesslayer.Context;
using nmct.project.tasktool.businesslayer.Repositories;
using nmct.project.tasktool.businesslayer.Repositories.Interfaces;
using nmct.project.tasktool.businesslayer.Services;
using nmct.project.tasktool.businesslayer.Services.Interfaces;
using nmct.project.tasktool.businesslayer.Services.TrelloCardHelper;
using nmct.project.tasktool.businesslayer.Services.TrelloCardHelper.Interfaces;
using nmct.project.tasktool.models;
using nmct.project.tasktool.models.Trello;
using nmct.project.tasktool.web.Controllers;
using nmct.project.tasktool.web.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.testing.DependInjection
{
    [TestClass]
    public class TestBase
    {
        public ITrelloRepository _trelloRepository { get; set; }
        public IGenericRepository<TrelloCampus> _trelloCampusRepository { get; set; }
        public IGenericRepository<TrelloMember> _repoTrelloMember { get; set; }
        public IStaffRepository _staffRepository { get; set; }
        public IRoleRepository _roleRepository { get; set; }
        public IGenericRepository<ExternalUser> _externalRepository { get; set; }
        public ITaskPerformanceRepository _taskPerformanceRepository { get; set; }
        public IReactionRuntimeRepository _reactionRuntimeRepository { get; set; }

        public IStaffService _staffService { get; set; }
        public ITrelloMemberService _trelloMemberService { get; set; }
        public ITrelloService _trelloService { get; set; }
        public IReportService _reportService { get; set; }


        public ITrelloCardInfoFinder _trelloCardInfoFinder { get; set; }
        public ITrelloCardExtraFinder _trelloCardExtraFinder { get; set; }
        public ITrelloCardTaskCreator _trelloCardTaskCreator { get; set; }

        public SettingsController _settingsController { get; set; }
        public OverviewController _overviewController { get; set; }
        public ArchiveController _archiveController { get; set; }
        public ErrorController _errorController { get; set; }
        public PrintController _printController { get; set; }


        public OverviewApiController _overviewApiController { get; set; }
        public ReportsApiController _reportsApiController { get; set; }
        public ArchiveApiController _archiveApiController { get; set; }

        public TestBase()
        {
            this._externalRepository = new GenericRepository<ExternalUser, ApplicationDbContext>();
            this._staffRepository = new StaffRepository();
            this._repoTrelloMember = new GenericRepository<TrelloMember, ApplicationDbContext>();
            this._roleRepository = new RoleRepository();
            this._reactionRuntimeRepository = new ReactionRuntimeRepository();
            this._trelloCampusRepository = new GenericRepository<TrelloCampus, ApplicationDbContext>();
            this._taskPerformanceRepository = new TaskPerformanceRepository();
            this._trelloRepository = new TrelloRepository();

            this._trelloCardInfoFinder = new TrelloCardInfoFinder(this._trelloRepository, this._trelloMemberService, this._staffService);
            this._trelloCardTaskCreator = new TrelloCardTaskCreator(this._trelloCardInfoFinder);
            this._trelloCardExtraFinder = new TrelloCardExtraFinder(this._trelloCardTaskCreator, this._trelloRepository);

            this._staffService = new StaffService(this._staffRepository, this._roleRepository, this._externalRepository);
            this._trelloMemberService = new TrelloMemberService(this._repoTrelloMember);
            this._reportService = new ReportService(this._taskPerformanceRepository, this._trelloCampusRepository, this._reactionRuntimeRepository);
            this._trelloService = new TrelloService(this._trelloRepository, this._trelloMemberService, this._staffService, this._reportService, this._trelloCardExtraFinder, this._trelloCardTaskCreator, this._trelloCardInfoFinder);

            this._printController = new PrintController(this._trelloService, this._staffService,
                this._trelloMemberService);
            this._overviewApiController = new OverviewApiController(this._trelloService, this._staffService, this._trelloCardExtraFinder);
            this._overviewController = new OverviewController(this._trelloService, this._trelloCardExtraFinder);
            this._errorController = new ErrorController();
            this._reportsApiController = new ReportsApiController(this._reportService);
            this._settingsController = new SettingsController(this._trelloService, this._staffService);
            this._archiveApiController = new ArchiveApiController(this._trelloService, this._reportService);
            this._archiveController = new ArchiveController(this._trelloService);
        }

    }
}
