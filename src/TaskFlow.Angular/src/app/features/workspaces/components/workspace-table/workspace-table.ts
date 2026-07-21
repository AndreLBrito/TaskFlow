import { DatePipe } from '@angular/common';
import { Component, input, output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';

import { Workspace } from '../../../../core/models/workspace';

@Component({
  selector: 'app-workspace-table',
  imports: [
    DatePipe,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatTooltipModule,
  ],
  templateUrl: './workspace-table.html',
  styleUrl: './workspace-table.scss',
})
export class WorkspaceTable {
  readonly workspaces = input<Workspace[]>([]);
  readonly editWorkspace = output<Workspace>();
  readonly deleteWorkspace = output<Workspace>();

  readonly displayedColumns = ['name', 'description', 'createdAt', 'actions'];
}
