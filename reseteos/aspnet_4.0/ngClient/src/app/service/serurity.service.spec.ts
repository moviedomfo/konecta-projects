import { TestBed } from '@angular/core/testing';

import { SerurityService } from './serurity.service';

describe('SerurityService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SerurityService = TestBed.get(SerurityService);
    expect(service).toBeTruthy();
  });
});
