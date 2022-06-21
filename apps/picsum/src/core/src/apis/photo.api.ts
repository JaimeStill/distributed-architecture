import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ServerConfig } from '../config';
import { Photo } from '../models';
import { QueryGeneratorService } from '../services';
import { EntityApi } from './entity.api';

const PICSUM_URL: string = `https://picsum.photos/`;

@Injectable({
  providedIn: 'root'
})
export class PhotoApi extends EntityApi<Photo> {
  constructor(
    protected config: ServerConfig,
    protected generator: QueryGeneratorService,
    protected http: HttpClient
  ) {
    super('photo', config, generator, http);
  }

  private ensureGreaterThanZero = (value: number) =>
    value > 0 ? value : 1;

  private getAspectHeight = (photo: Photo, w: number) =>
    this.ensureGreaterThanZero(
      Math.floor(
        w * (photo.height / photo.width)
      )
    );

  private getAspectWidth = (photo: Photo, h: number) =>
    this.ensureGreaterThanZero(
      Math.floor(
        h * (photo.width / photo.height)
      )
    );

  preview = (photo: Photo): string => {
    const width = photo.width > photo.height
      ? 300
      : this.getAspectWidth(photo, 200);
    const height = photo.height > photo.width
      ? 200
      : this.getAspectHeight(photo, 300);


    return `${PICSUM_URL}id/${photo.picsumId}/${width}/${height}.webp`;
  }

  queryByAuthor = (author: string) =>
    this.generator.generateSource<Photo>(
      'id',
      `${this.endpoint}queryByAuthor/${author}`
    );

  seedByObservable = (): Promise<boolean> => new Promise((resolve, reject) => {
      this.http.get(`${this.api}seedByObservable`)
        .subscribe({
          next: () => resolve(true),
          error: (err: any) => reject(err)
        })
  });

  seedByStream = (): Promise<boolean> => new Promise((resolve, reject) => {
    this.http.get(`${this.api}seedByStream`)
      .subscribe({
        next: () => resolve(true),
        error: (err: any) => reject(err)
      })
  });

  validate = (photo: Photo): Promise<boolean> =>
    this.execute(
      this.http.post<boolean>(`${this.api}validate`, photo)
    );
}
