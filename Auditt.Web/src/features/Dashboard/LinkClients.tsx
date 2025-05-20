import { SimpleNavItem } from "../../shared/components/Navigation/NavVerticalItem";

export const LinkClients = () => {
    return (
      <div className="flex  mb-3">
        <SimpleNavItem to="/Clients" title="Clientes"/>
        <SimpleNavItem to="/patients" title="Pacientes"/>
        <SimpleNavItem to="/Functionary" title="Profesionales"/>
      </div>
    );
  };