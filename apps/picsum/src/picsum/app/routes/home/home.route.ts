import {
  Component,
  OnDestroy,
  OnInit
} from '@angular/core';

import {
  Photo,
  PhotoApi,
  QuerySource
} from 'core';

@Component({
  selector: 'home-route',
  templateUrl: 'home.route.html'
})
export class HomeRoute implements OnInit, OnDestroy {
  photoSrc: QuerySource<Photo>;
  seeding: boolean = false;

  constructor(
    public photoApi: PhotoApi
  ) { }

  ngOnInit(): void {
    this.photoSrc = this.photoApi.query();
  }

  ngOnDestroy(): void {
    this.photoSrc.unsubscribe();
  }

  seed = async () => {
    this.seeding = true;
    const res = await this.photoApi.seedByStream();
    res && this.photoSrc.forceRefresh();
    this.seeding = false;
  }
}
