import { useState } from "react";
import OffCanvas from "../../shared/components/OffCanvas/Index";
import { Direction } from "../../shared/components/OffCanvas/Models";
import ButtonDelete from "../../shared/components/Buttons/ButtonDelete";
import { Bar } from "../../shared/components/Progress/Bar";
import { MouseEvent } from "react";
import Swal from "sweetalert2";
import { ClientModel } from "./ClientModel";
import { ClientCreate } from "./ClientCreate";
import { ClientUpdate } from "./ClientUpdate";
import { useClient } from "./useClient";
import { LinkClients } from "../Dashboard/LinkClients";
import { ButtonUpdate } from "../../shared/components/Buttons/ButtonDetail";
export const Clients = () => {
    const [visible, setVisible] = useState(false);
    const [visibleUpdate, setVisibleUpdate] = useState(false);
    const { clients, queryClients, deleteClient } = useClient();
    const [client, setClient] = useState<ClientModel>();

    const handleClickDetail = (clientSelected: ClientModel) => {
        if (clientSelected) {
            setClient(clientSelected);
            setVisibleUpdate(true);
        }
    }

    function handleDelete(e: MouseEvent<HTMLButtonElement>, id: number): void {
        e.preventDefault();
        Swal.fire({
            title: '¿Estás seguro de eliminar este cliente?',
            text: 'Esta acción no se puede deshacer',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Confirmar',
            cancelButtonText: 'Cancelar',
            preConfirm: async () => {
                await deleteClient.mutateAsync(id);
            }
        })
    }

    if (queryClients.isLoading)
        return <Bar />

    return (
        <div className="flex p-8 w-full">
            <div>
                <div className="flex space-x-8 text-lg font-medium mb-6 mr-2">
                    <LinkClients />
                </div>

                <h2 className="text-2xl font-semibold mb-4">Clientes o Instituciones</h2>

                <button onClick={() => setVisible(true)} className="bg-[#392f5a] hover:bg-indigo-900 text-white px-6 py-2 rounded-lg font-semibold mb-2">
                    Crear Clientes
                </button>
                <div>
                    <div className="grid grid-cols-[2fr_3fr_3fr_3fr_1fr] w-full">
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">RAZON SOCIAL</div>
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">ABREVIATURA</div>
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">NIT</div>
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">CIUDAD</div>
                        <div className=" font-semibold bg-gray-300  text-gray-800 px-2 py-1 text-center">OPCIONES</div>
                    </div>
                    <div className=" bg-white px-2 py-2 border border-gray-200">
                        {clients?.map((client) => (
                            <div className="grid grid-cols-[2fr_3fr_3fr_3fr_1fr] w-full hover:bg-[#F4EDEE] transition-colors">
                                <div className="gap-3 text-sm px-2 py-2 border border-gray-300">{client.name}</div>
                                <div className="gap-3 text-sm px-2 py-2 border border-gray-300">{client.abbreviation}</div>
                                <div className="gap-3 text-sm px-2 py-2 border border-gray-300 text-center">{client.nit}</div>
                                <div className="gap-3 text-sm px-2 py-2 border border-gray-300 mr-">{client.city}</div>
                                <div className="flex text-sm px-2  border border-gray-300">
                                    <ButtonDelete id={client.id ?? 0} onDelete={handleDelete} />
                                    <div onClick={() => handleClickDetail(client)}>
                                        <ButtonUpdate />
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
                <OffCanvas titlePrincipal='Crear Cliente' visible={visible} xClose={() => setVisible(false)} position={Direction.Right}  >
                    <ClientCreate />
                </OffCanvas>
                {
                    client &&
                    <OffCanvas titlePrincipal='Detalle Cliente' visible={visibleUpdate} xClose={() => setVisibleUpdate(false)} position={Direction.Right}  >
                        <ClientUpdate data={client} />
                    </OffCanvas>
                }
            </div>
        </div>
    );
}