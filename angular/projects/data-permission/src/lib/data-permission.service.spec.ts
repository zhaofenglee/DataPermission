import { TestBed } from '@angular/core/testing';
import { DataPermissionService } from './services/data-permission.service';
import { RestService } from '@abp/ng.core';

describe('DataPermissionService', () => {
  let service: DataPermissionService;
  const mockRestService = jasmine.createSpyObj('RestService', ['request']);
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: RestService,
          useValue: mockRestService,
        },
      ],
    });
    service = TestBed.inject(DataPermissionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
