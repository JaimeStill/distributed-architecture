import { EntityBase } from '../entity-base';

export interface Photo extends EntityBase {
  picsumId: number;
  author: string;
  url: string;
  downloadUrl: string;
  width: number;
  height: number;
}
