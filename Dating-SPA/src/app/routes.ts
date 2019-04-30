import { MemberListComponent } from './Member-list/Member-list.component';
import { HomeComponent } from './home/home.component';
import { Routes } from '@angular/router';
import { MessagesComponent } from './Messages/Messages.component';
import { ListsComponent } from './Lists/Lists.component';

export const appRoutes: Routes = [
    {path: 'home', component: HomeComponent},
    {path: 'members', component: MemberListComponent},
    {path: 'messages', component: MessagesComponent},
    {path: 'lists', component: ListsComponent},
    {path: '**', redirectTo: 'home', pathMatch: 'full'},
];

