export const estados = {
  1: { label: "Activo", textColor: "text-lime-600", dotColor: "bg-lime-500" },
  2: { label: "Inactivo", textColor: "text-red-600", dotColor: "bg-red-500" }
} as const;

type EstadoId = keyof typeof estados;


export const UserStatusLabel = ({ idEstado }: { idEstado: number }) => {
  const estado = estados[idEstado as EstadoId] ?? {
    label: "Desconocido",
    textColor: "text-gray-400",
    dotColor: "bg-gray-400"
  };

  return (
    <span className={`flex items-center gap-2 text-sm font-semibold ${estado.textColor}`}>
      <div className={`w-4 h-4 rounded-full ${estado.dotColor}`} />
      {estado.label}
    </span>
  );
};