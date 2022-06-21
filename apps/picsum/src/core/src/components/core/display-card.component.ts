import {
  Component,
  Input
} from '@angular/core';

@Component({
  selector: 'display-card',
  templateUrl: 'display-card.component.html'
})
export class DisplayCardComponent {
  @Input() size: number = 360;
  @Input() cardStyle: string = 'm4 p4 background-card card-outline-accent rounded';
}
